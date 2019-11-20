using System;
using ReplicationUtilities.Messaging;

namespace Consumer
{
    class Program
    {
        static void Main()
        {
            string path = @"D:\destination";
            ICentralServerService centralServerService = new CentralServerService(path, new RabbitMqHelper());
            centralServerService.Connect();
            Console.WriteLine($" [*] CentralServerService is started. Destination folder: {path}");

            Console.WriteLine(" Press [Esc] to stop service and exit.");
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
            centralServerService.Disconnect();
        }
    }
}
