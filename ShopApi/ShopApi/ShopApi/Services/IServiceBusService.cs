using ShopApi.Models;

namespace ShopApi.Services;

public interface IServiceBusService
{
    void ProductAdded(Product product);
}
