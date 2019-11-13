using RabbitMQ.Client;

namespace ReplicationUtilities.Messaging
{
    public interface IRabbitMqHelper
    {
        IModel CreateChannel(string updateExchange);
        void Dispose();
    }
}