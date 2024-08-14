using System.Text.Json.Serialization;

namespace ShopApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
    [JsonIgnore]
    public virtual Order Order { get; set; }
}
