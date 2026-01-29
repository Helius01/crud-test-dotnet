using Customers.Api.Endpoints;
using Customers.Application.Abstractions;
using Customers.Application.ReadModel;
using Customers.Infrastructure.EventStore;
using Customers.Infrastructure.Projections;
using Customers.Infrastructure.ReadModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Customers.Application.Commands.CreateCustomer.CreateCustomerCommand).Assembly));
builder.Services.AddSingleton<IEventStore, InMemoryEventStore>();
builder.Services.AddSingleton<ICustomerReadStore, InMemoryCustomerReadStore>();
builder.Services.AddSingleton<CustomerProjection>();
builder.Services.AddSingleton<IEventDispatcher, InMemoryEventDispatcher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapCustomers();
app.MapGet("/", () => Results.Ok("Customers API is running"));

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
