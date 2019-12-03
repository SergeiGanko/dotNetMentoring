using System;
using Autofac;

namespace Consumer
{
    class Program
    {
        static void Main()
        {
            IContainer container = Bootstrapper.Bootstrap();
            ICentralServerService centralServerService = container.Resolve<ICentralServerService>();
            centralServerService.Connect();

            Console.WriteLine(" Press [Esc] to stop service and exit.");
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
            centralServerService.Disconnect();
        }
    }
}
