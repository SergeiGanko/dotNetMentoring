using System;
using ReplicationUtilities.Messaging;

namespace Producer
{
    public sealed class Program
    {
        static void Main()
        {
            string path = @"D:\source";
            IInputService inputService = new InputService(path, new RabbitMqHelper());
            inputService.Connect();
            Console.WriteLine($" [*] InputService is started. Source folder: {path}");

            Console.WriteLine(" Press [Esc] to stop service and exit.");
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
            inputService.Disconnect();
        }
    }
}
