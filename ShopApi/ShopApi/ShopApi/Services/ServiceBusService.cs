using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using ShopApi.Models;
using System.Text;
using System.Text.Json;

namespace ShopApi.Services;

public class ServiceBusService(IAzureClientFactory<ServiceBusClient> serviceBusClientFactory) : IServiceBusService
{
    public void ProductAdded(Product product)
    {
        var client = serviceBusClientFactory.CreateClient("servicebus_client");
        var sender = client.CreateSender(queueOrTopicName: "product-added");

        var message=new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(product)));
        sender.SendMessageAsync(message);
    }
}
