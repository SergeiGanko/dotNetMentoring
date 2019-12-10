using Autofac;
using ReplicationUtilities.Messaging;

namespace Consumer
{
    public class Bootstrapper
    {
        public static IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CentralServerService>().As<ICentralServerService>();
            builder.RegisterType<RabbitMqHelper>().As<IRabbitMqHelper>();

            return builder.Build();
        }
    }
}