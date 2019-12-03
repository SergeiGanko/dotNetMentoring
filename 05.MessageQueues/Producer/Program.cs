using System;
using Autofac;

namespace Producer
{
    public sealed class Program
    {
        static void Main()
        {
            IContainer container = Bootstrapper.Bootstrap();
            IInputService inputService = container.Resolve<IInputService>();
            inputService.Connect();

            Console.WriteLine(" Press [Esc] to stop service and exit.");
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
            inputService.Disconnect();
        }
    }
}
