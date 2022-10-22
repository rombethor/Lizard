using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Lizard.Messaging
{
    public class MessageQueueListener
    {
        IConnection connection;
        readonly IModel channel;

        const string queueName = "lizard";
        
        readonly EventingBasicConsumer logConsumer;
        
        string _dbConnectionString = string.Empty;

        public MessageQueueListener(IConfiguration config)
        {
            _dbConnectionString = config["database"];

            string rabbithost = config["rabbithost"] ?? string.Empty;
            string rabbituser = config["rabbituser"] ?? string.Empty;
            string rabbitpass = config["rabbitpass"] ?? string.Empty;

            if (rabbithost == string.Empty || rabbituser == string.Empty || rabbitpass == string.Empty)
            {
                throw new Exception("The following connection details need to be filled in for RabbitMQ: rabbithost, rabbituser, rabbitpass");
            }

            var factory = new ConnectionFactory()
            {
                HostName = config["rabbithost"],
                UserName = config["rabbituser"],
                Password = config["rabbitpass"]
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.BasicQos(0, 20, false);

            var args = new Dictionary<string, object?>();
            args.Add("x-message-ttl", 3600000);

            channel.QueueDeclare(queueName, true, false, false, args);

            logConsumer = new EventingBasicConsumer(channel);
            logConsumer.Received += LogConsumer_Received;
            
            channel.BasicConsume(queueName, false, logConsumer);
        }

        private void LogConsumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            //deserialise response
            var strBody = Encoding.UTF8.GetString(e.Body.ToArray());
            var options = JsonSerializer.Deserialize<Models.LogEntryAddOptions>(strBody);
            if (options != null)
            {
                DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
                optionsBuilder.UseSqlServer(_dbConnectionString);
                var db = new LizardDbContext(optionsBuilder.Options);

                //check for source
                long sourceID = db.Sources.Where(s => s.Name == options.Source.Name && s.Version == options.Source.Version)
                    .Select(s => s.SourceID)
                    .FirstOrDefault();

                if (sourceID == 0)
                {
                    var source = new Entities.Source()
                    {
                        Name = options.Source.Name,
                        Version = options.Source.Version
                    };
                    db.Sources.Add(source);
                    sourceID = source.SourceID;
                }

                //check for duplicates
                var hash = LogEntryHash.Compute(options);
                var duplicate = db.LogEntries.FirstOrDefault(le => le.SHA256 == hash);
                if (duplicate != null)
                {
                    db.Occurrences.Add(new Entities.Occurrence()
                    {
                        LogEntryID = duplicate.LogEntryID,
                        Occurred = options.Occurred
                    });
                    db.SaveChanges();
                }
                else
                { //no duplicate
                    var entity = new Entities.LogEntry()
                    {
                        SourceID = sourceID,
                        SHA256 = hash,
                        Message = options.Message,
                        Occurrences = new Entities.Occurrence[] {
                            new Entities.Occurrence(){ Occurred = options.Occurred, Written = DateTime.UtcNow }
                        }
                    };

                    if (options.Exception != null)
                    {
                        //create exception log entry
                        entity.Exception = new Entities.ExceptionLogEntry()
                        {
                            StackTrace = options.Exception?.StackTrace != null ? new Entities.StackTrace()
                            {
                                Content = options.Exception!.StackTrace!.Content,
                                TargetSite = options.Exception!.StackTrace!.MethodName
                            } : null
                        };

                        //TODO: Inner exception
                    }
                    if (options.HttpDetail != null)
                    {
                        entity.HttpRequest = new Entities.HttpRequestLogEntry()
                        {
                            Method = options.HttpDetail.Method,
                            Uri = options.HttpDetail.Uri,
                            Content = options.HttpDetail.RequestBody ?? string.Empty
                        };
                        if (options.HttpDetail.StatusResult != null)
                        {
                            entity.HttpRequest.Response = new Entities.HttpResponseLogEntry()
                            {
                                StatusCode = options.HttpDetail.StatusResult ?? 0,
                                Content = options.HttpDetail.ResponseBody ?? string.Empty
                            };
                        }
                    }
                    db.LogEntries.Add(entity);
                    db.SaveChanges();
                }

            }
            channel.BasicAck(e.DeliveryTag, false);
        }
    }
}
