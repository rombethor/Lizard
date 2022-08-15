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

        private static LizardMessageClient? _instance;

        /// <summary>
        /// The single instance of the client.  Initialise with <see cref="Initialise(string, string, string)"/>
        /// </summary>
        public static LizardMessageClient Instance => _instance ?? throw new Exception("LizardMessageClient not initialised.");

        /// <summary>
        /// Initialise the <see cref="Instance"/> of the client by providing the RabbitMQ detail.
        /// </summary>
        /// <param name="host">RabbitMQ host address</param>
        /// <param name="user">RabbitMQ exchange username</param>
        /// <param name="password">RabbitMQ exchange password</param>
        public static void Initialise(string host, string user, string password)
        {
            _instance = new LizardMessageClient(host, user, password);
        }

        private LizardMessageClient(string host, string user, string password)
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
            channel.BasicPublish("", "lizard", null, data);
        }

        public void SendLog(Models.LogEntryAddOptions options)
        {
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(options)).ToArray();
            channel.BasicPublish("", "lizard", null, data);
        }

    }
}