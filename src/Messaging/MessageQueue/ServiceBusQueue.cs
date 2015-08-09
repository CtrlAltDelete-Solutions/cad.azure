using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace CAD.Azure
{
    public class ServiceBusQueue : IMessageQ
    {
        private readonly string _connectionString;

        public ServiceBusQueue(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public void Initialize(string queueName)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(this._connectionString);
            //check if queue name exists
            if (!namespaceManager.QueueExists(queueName))
            {
                QueueDescription queue = new QueueDescription(queueName);
                //queue.MaxSizeInMegabytes = 5120; //5GB
                //expiry to 30 days
                //queue.DefaultMessageTimeToLive = new System.TimeSpan(720, 0, 0);

                namespaceManager.CreateQueue(queue);
            }
        }

        public void Send(string queueName, object message)
        {
            //initialize queue
            this.Initialize(queueName);

            //create brokered message 
            BrokeredMessage brokeredMessage = new BrokeredMessage(message);
            

            QueueClient client = QueueClient.CreateFromConnectionString(this._connectionString, queueName);
            client.Send(brokeredMessage);
            
        }
    }
}
