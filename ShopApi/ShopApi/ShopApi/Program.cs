using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using ShopApi.Models;
using ShopApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IServiceBusService, ServiceBusService>();
builder.Services.AddAzureClients(clients =>
{
    clients.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus")).WithName("servicebus_client");
});
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    //opt.UseInMemoryDatabase("ShopDb");
    var connectionString = builder.Configuration.GetConnectionString("SqlServer");
    opt.UseSqlServer(connectionString, opt =>
    {
        opt.UseAzureSqlDefaults(true);
    });
});
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", async ([FromServices] AppDbContext context) =>
{
    return await context.Products.ToListAsync();
});
app.MapGet("/test", () =>
{
    return "Test worked ";
});
app.MapPost("/products", async ([FromServices] AppDbContext context, [FromBody] Product product) =>
{
    context.Products.Add(product);
    await context.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapGet("/orders", async ([FromServices] AppDbContext context) =>
{
    return await context.Orders.ToListAsync();
});
app.MapPost("/orders", async ([FromServices] AppDbContext context, [FromBody] Order order) =>
{
    context.Orders.Add(order);
    await context.SaveChangesAsync();
    return Results.Created($"/orders/{order.Id}",order);

});
app.MapPost("/cart", async ([FromServices] IServiceBusService serviceBusService, [FromBody] Product product) =>
{
    serviceBusService.ProductAdded(product);
    return Results.Accepted();
});

using (var scope = app.Services.CreateScope())
{
    var dbContext=scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if(dbContext.Database.IsInMemory())
        dbContext.Database.EnsureCreated();
}

app.Run();