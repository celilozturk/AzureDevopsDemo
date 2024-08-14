using System;
using System.IO;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ShopCart.Function
{
    public class ProductFunction()
    {
        
        [FunctionName("ProductAdded")]
        [return: ServiceBus("product-processed",Connection ="ServiceBus")]
        public string ProductAdded([ServiceBusTrigger("product-added", Connection = "ServiceBus")]ServiceBusReceivedMessage message, ILogger log)
        {
            log.LogInformation("New release isValid!");
            log.LogInformation("Message ID: {id}",message.MessageId);
            log.LogInformation("Message Body : {body}",message.Body);
            log.LogInformation("Message Content-Type: {contentType}",message.ContentType);
            using var reader = new StreamReader(message.Body.ToStream());
            return reader.ReadToEnd();

        }
    }
}
