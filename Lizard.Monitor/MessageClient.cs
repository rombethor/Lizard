using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Lizard.Monitor
{
    /// <summary>
    /// Client for sending log entries, exceptions and http logs
    /// via RabbitMQ
    /// </summary>
    public class LizardMessageClient
    {
        readonly IConnection connection;
        readonly IModel channel;

        public LizardMessageClient(string host, string user, string password)
        {
            var factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = password
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void SendException(Exception ex)
        {
            var exc = new Models.ExceptionAddOptions(ex);
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(exc)).ToArray();
            channel.BasicPublish("lizard", "log", null, data);
        }

    }
}