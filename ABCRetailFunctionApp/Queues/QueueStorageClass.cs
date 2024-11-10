using Azure.Storage.Queues;
using System;
using System.Threading.Tasks;

namespace ABCRetailFunctionApp.Queues
{
    public abstract class QueueStorageClass
    {
        // Data fields
        private readonly string connectionString;
        private readonly string containerName;

        /// <summary>
        /// Master constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="containerName"></param>
        public QueueStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        /// <summary>
        /// Adds a message to the container.
        /// </summary>
        /// <param name="message">The message content to add.</param>
        /// <returns></returns>
        public async Task<string> AddMessage(string message)
        {
            // Declare and instantiate a QueueClient object.
            QueueClient client = new QueueClient(connectionString, containerName);

            return client?.SendMessageAsync(message).Result.Value?.MessageId;
        }

        /// <summary>
        /// Deletes a message from the container by the message id.
        /// </summary>
        /// <param name="id">The id of the message.</param>
        public void DeleteMessage(string id)
        {
            // Declare and instantiate a QueueClient object.
            QueueClient client = new QueueClient(connectionString, containerName);
            // Recieve a collection of messages from the client.
            var messages = client.ReceiveMessages();

            // Iterate through the message collection
            foreach (var message in messages.Value)
            {
                // Find the message by checking its id.
                if (message.MessageId == id)
                {
                    // Delete the message from the container.
                    client.DeleteMessageAsync(message.MessageId, string.Empty);
                    break;
                }
            }
        }

        public string this[string messageId]
        {
            get
            {
                QueueClient client = new QueueClient(connectionString, containerName);
                var messages = client.ReceiveMessages().Value;
                string result = string.Empty;

                foreach(var message in messages)
                {
                    if(message.MessageId == messageId)
                    {
                        result = message.Body.ToString();
                        break;
                    }
                }

                return result;
            }
            set
            {
                QueueClient client = new QueueClient(connectionString, containerName);
                var messages = client.ReceiveMessages().Value;
                string result = string.Empty;

                foreach (var message in messages)
                {
                    if (message.MessageId == messageId)
                    {
                        client.UpdateMessage(message.MessageId, message.PopReceipt, new BinaryData(value));
                        break;
                    }
                }
            }
        }
    }
}
