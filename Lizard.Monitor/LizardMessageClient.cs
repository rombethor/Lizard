using RabbitMQ.Client;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

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

        /// <summary>
        /// Send an exception to be logged
        /// </summary>
        /// <param name="ex"></param>
        public void SendException(Exception ex)
        {
            var log = new Models.LogEntryAddOptions()
            {
                Exception = new Models.ExceptionAddOptions(ex),
                Message = ex.Message,
                Occurred = DateTime.UtcNow,
                Source = GetSource()
            };
            //var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(log)).ToArray();
            //channel.BasicPublish("", "lizard", null, data);
            SendLog(log);
        }

        /// <summary>
        /// Send a message to be logged
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            var log = new Models.LogEntryAddOptions()
            {
                Source = GetSource(),
                Message = message,
                Occurred = DateTime.UtcNow
            };
            SendLog(log);
        }

        /// <summary>
        /// Send a custom log entry
        /// </summary>
        /// <param name="options"></param>
        public void SendLog(Models.LogEntryAddOptions options)
        {
            if (options.Source == null)
                options.Source = GetSource();
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(options)).ToArray();
            channel.BasicPublish("", "lizard", null, data);
        }

        private Models.SourceAddOptions GetSource()
        {
            var name = Assembly.GetEntryAssembly()?.GetName();
            var source = new Models.SourceAddOptions()
            {
                Name = name?.Name ?? string.Empty,
                Version = name?.Version?.ToString() ?? string.Empty
            };
            return source;
        }

    }
}