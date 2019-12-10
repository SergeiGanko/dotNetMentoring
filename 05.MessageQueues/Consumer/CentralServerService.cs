using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReplicationUtilities.Messaging;
using ReplicationUtilities.Models;

namespace Consumer
{
    public class CentralServerService : ICentralServerService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string DirectoryPath = @"D:\destination";

        private readonly IRabbitMqHelper _rabbitMqHelper;
        private readonly IModel _channel;
        
        private List<Chunk> _chunks = new List<Chunk>();

        public CentralServerService(IRabbitMqHelper rabbitMqHelper)
        {
            _rabbitMqHelper = rabbitMqHelper;
            // Create connection, channel and declare exchange
            _channel = _rabbitMqHelper.CreateChannel(RabbitMqConstants.RabbitMqExchange);
        }

        [LoggingAspect]
        public void Connect()
        {
            if (_channel.IsOpen)
            {
                // Declare queue
                var queueName = _channel
                    .QueueDeclare(RabbitMqConstants.RabbitMqQueue, true, false, false, null)
                    .QueueName;

                // Don't dispatch a new message to a consumer until it has processed and acknowledged the previous one
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                // Create binding between queue and exchange
                _channel.QueueBind(queueName, RabbitMqConstants.RabbitMqExchange, string.Empty, null);

                // Provide a callback to consume asynchronous messages from RabbitMq
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += MessageReceived;

                // Start consuming messages from queue
                _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                Console.WriteLine($" [*] CentralServerService is started. Destination folder: {DirectoryPath}");
            }
            else
            {
                throw new Exception("RabbitMq broker server unavailable!");
            }
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs e)
        {
            IDictionary<string, object> headers = e.BasicProperties.Headers;
            var fileName = Encoding.UTF8.GetString(headers[nameof(Chunk.FileName)] as byte[]);
            var index = (int) headers[nameof(Chunk.Index)];
            var isLastChunk = (bool) headers[nameof(Chunk.IsLastChunk)];

            Logger.Info($"Received a chunk #{index} of file '{fileName}'. IsLastChunk: {isLastChunk}");

            var chunk = new Chunk
            {
                FileName = fileName,
                Content = e.Body,
                Index = index,
                IsLastChunk = isLastChunk
            };
            _chunks.Add(chunk);

            if (chunk.IsLastChunk)
            {
                var chunksGropedByFileName = _chunks
                    .GroupBy(c => c.FileName)
                    .ToDictionary(c => c.Key, c => c.ToList());

                if (chunksGropedByFileName.TryGetValue(chunk.FileName, out var chunkList))
                {
                    var bytes = CombineChunks(chunkList);
                    SaveToFile(fileName, bytes);

                    // keep chunks that have not consumed yet and get rid of saved chunks
                    var remainingChunks = _chunks.Where(c => c.FileName != chunk.FileName);
                    _chunks = new List<Chunk>(remainingChunks);
                }
            }
            _channel.BasicAck(e.DeliveryTag, false);
        }

        private byte[] CombineChunks(IEnumerable<Chunk> chunks)
        {
            var arrays = chunks
                .OrderBy(c => c.Index)
                .Select(c => c.Content)
                .ToArray();
            
            var offset = 0;
            var destination = new byte[arrays.Sum(c => c.Length)];

            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, destination, offset, data.Length);
                offset += data.Length;
            }

            return destination;
        }

        private void SaveToFile(string fileName, byte[] content)
        {
            var fullPath = Path.Combine(DirectoryPath, $"{fileName}.pdf");
            File.WriteAllBytes(fullPath, content);
        }

        [LoggingAspect]
        public void Disconnect()
        {
            _rabbitMqHelper?.Dispose();
        }
    }
}