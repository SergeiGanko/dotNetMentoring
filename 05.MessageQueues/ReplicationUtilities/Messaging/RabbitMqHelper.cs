using System;
using Autofac.Extras.DynamicProxy;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReplicationUtilities.Messaging
{
    [Intercept("method-tracer")]
    public class RabbitMqHelper : IRabbitMqHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IConnection _connection;

        public RabbitMqHelper()
        {
            var factory = new ConnectionFactory
            {
                Protocol = Protocols.AMQP_0_9_1,
                HostName = "localhost",
                AutomaticRecoveryEnabled = true
            };

            Logger.Info($"Creating RabbitMQ connection to {factory.HostName}:{factory.Port}");
            _connection = factory.CreateConnection();
            _connection.CallbackException += _connection_CallbackException;
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;
            _connection.ConnectionRecoveryError += _connection_ConnectionRecoveryError;
            _connection.RecoverySucceeded += _connection_RecoverySucceeded;
        }
        
        private IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public IModel CreateChannel(string updateExchange)
        {
            var channel = CreateChannel();
            Logger.Info($"Declaring an exchange: Name - {updateExchange}, Type - {ExchangeType.Fanout}");
            channel.ExchangeDeclare(updateExchange, ExchangeType.Fanout, true, false, null);
            return channel;
        }

        public void Dispose()
        {
            _connection.ConnectionRecoveryError -= _connection_ConnectionRecoveryError;
            _connection.ConnectionShutdown -= _connection_ConnectionShutdown;
            _connection.CallbackException -= _connection_CallbackException;
            _connection.RecoverySucceeded -= _connection_RecoverySucceeded;
            _connection.Close();
        }

        private void _connection_RecoverySucceeded(object sender, EventArgs e)
        {
            Logger.Info("RabbitMq Connection is recovered");
        }

        private void _connection_ConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
            Logger.Error(e.Exception, "RabbitMQ connection recovery error");
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Logger.Error("RabbitMQ connection is shutting down");
        }

        private void _connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "RabbitMQ callback exception is thrown");
        }
    }
}