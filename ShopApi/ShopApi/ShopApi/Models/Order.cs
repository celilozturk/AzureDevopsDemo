﻿namespace ShopApi.Models;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}
