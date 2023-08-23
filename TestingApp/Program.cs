using Azure.Messaging.ServiceBus;
using Data;
using Data.Repositories;
using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddTransient<IMessageBroker, MessageBroker>();

var settings = builder.Configuration.Get<Settings>();
builder.Services.AddSingleton(new ServiceBusClient(settings.ServiceBus.PrimaryConnectionString));
builder.Services.AddSingleton(settings);

builder.Services.AddDbContext<Context>(options => options.UseSqlServer(settings.Database.ConnectionString));

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IRepository<Order>, Repository<Order>>();

builder.Services.AddTransient<IValidator<CustomerRequest>, CustomerValidator>();
builder.Services.AddTransient<IValidator<OrderRequest>, OrderValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
