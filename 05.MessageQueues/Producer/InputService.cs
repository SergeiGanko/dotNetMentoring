using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using RabbitMQ.Client;
using ReplicationUtilities.Messaging;
using ReplicationUtilities.Models;


namespace Producer
{
    public class InputService : IInputService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly NotifyFilters filter = NotifyFilters.FileName |
                                                NotifyFilters.LastWrite |
                                                NotifyFilters.CreationTime;

        private readonly FileSystemWatcher _fileWatcher;
        private readonly IRabbitMqHelper _rabbitMqHelper;
        private readonly IModel _channel;

        private object _lock = new object();

        public InputService(string directoryPath, IRabbitMqHelper rabbitMqHelper)
        {
            _fileWatcher = new FileSystemWatcher
            {
                Path = directoryPath,
                Filter = "*.pdf",
                NotifyFilter = filter
            };

            _rabbitMqHelper = rabbitMqHelper;
            // Create connection, channel and declare exchange
            _channel = _rabbitMqHelper.CreateChannel(RabbitMqConstants.RabbitMqExchange);
        }

        public void Connect()
        {
            _fileWatcher.Created += OnCreated;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (_channel.IsOpen)
            {
                Task.Run(async () =>
                {
                    // Ensure queue exists
                    _channel.QueueDeclare(RabbitMqConstants.RabbitMqQueue, true, false, false, null);

                    // Enable publisher confirms at the channel level.
                    // When publisher confirms are enabled on a channel, messages the client publishes are
                    // confirmed asynchronously by the broker, meaning they have been taken care of on the server side
                    _channel.ConfirmSelect();

                    try
                    {
                        var timer = new Stopwatch();
                        timer.Start();
                        Logger.Info($" [*] Processing pdf-file '{e.Name}'...");
                        if (!WaitUntilFileAvailableToRead(e.FullPath, attemts: 20))
                        {
                            throw new FieldAccessException($"File {e.Name} cannot be read.");
                        }

                        var bytes = await File.ReadAllBytesAsync(e.FullPath);
                        int chunkSize = 1000000;

                        lock (_lock)
                        {
                            SendMessage(bytes, e.Name, chunkSize);
                        }

                        timer.Stop();
                        Console.WriteLine($" [*] Elapsed: {timer.ElapsedMilliseconds:N0} ms");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            }
            else
            {
                Disconnect();
                throw new Exception("RabbitMq broker server unavailable!");
            }
        }

        private bool WaitUntilFileAvailableToRead(string filePath, int attemts = 20)
        {
            bool result = false;
            int i = 0;
            do
            {
                try
                {
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        Logger.Info($"File '{filePath}' can be read.");
                        result = fs != null;
                        break;
                    }
                }
                catch (IOException ex)
                {
                    Logger.Error($"File '{filePath}' is not ready yet. Error message: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"File '{filePath}' is not ready yet. Error message: {ex.Message}");
                }

                Logger.Warn("Retry after 0.5 second wait...");
                Thread.Sleep(500);
                i++;
            } while (i != attemts);

            return result;
        }

        private void SendMessage(byte[] message, string fileName, int chunkSize)
        {
            var chunks = DivideToChunks(message, fileName, chunkSize);
            foreach (var chunk in chunks)
            {
                SendChunk(chunk);
            }
        }

        private void SendChunk(Chunk chunk)
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object>
            {
                {nameof(Chunk.FileName), chunk.FileName},
                {nameof(Chunk.IsLastChunk), chunk.IsLastChunk},
                {nameof(Chunk.Index), chunk.Index}
            };

            Logger.Info($" [*] Publishing chunk #{chunk.Index} of file '{chunk.FileName}'. IsLastChunk: {chunk.IsLastChunk}");
            _channel.BasicPublish(
                RabbitMqConstants.RabbitMqExchange,
                RabbitMqConstants.RabbitMqQueue,
                properties, chunk.Content);

            _channel.WaitForConfirmsOrDie();
        }

        private List<Chunk> DivideToChunks(byte[] message, string fileName, int chunkSize)
        {
            var chunks = new List<Chunk>();
            var length = message.Length;
            if (length == 0)
            {
                return chunks;
            }

            int pos = 0;
            int i = 0;
            while (true)
            {
                var size = Math.Min(chunkSize, length - pos);
                var body = new ArraySegment<byte>(message, pos, size);
                pos += size;

                if (pos == length)
                {
                    chunks.Add(new Chunk
                    {
                        Content = body.ToArray(),
                        FileName = fileName,
                        Index = i,
                        IsLastChunk = true
                    });
                    break;
                }

                chunks.Add(new Chunk
                {
                    Content = body.ToArray(),
                    FileName = fileName,
                    Index = i,
                    IsLastChunk = false
                });
                i++;
            }

            return chunks;
        }

        public void Disconnect()
        {
            _fileWatcher.Created -= OnCreated;
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher?.Dispose();
            _rabbitMqHelper?.Dispose();
        }
    }
}