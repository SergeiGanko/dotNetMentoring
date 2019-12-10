using AOP.Logging.CastleCore;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using ReplicationUtilities.Messaging;

namespace Producer
{
    public class Bootstrapper
    {
        public static IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<InputService>().As<IInputService>().EnableInterfaceInterceptors();
            builder.RegisterType<RabbitMqHelper>().As<IRabbitMqHelper>().EnableInterfaceInterceptors();
            builder.Register(c => new LoggingInterceptor()).Named<IInterceptor>("method-tracer");

            return builder.Build();
        }
    }
}