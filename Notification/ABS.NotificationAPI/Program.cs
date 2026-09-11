using ABS.Notification.Application.Consumers;
using ABS.Notification.Core.Repositories;
using ABS.Notification.Infrastructure.Repositories;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Transit.Common;
using FluentValidation;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// SQL Connection for Dapper
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));


// Application Services MediatR
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(assembly);
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));
	config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Add FluentValidation services
builder.Services.AddValidatorsFromAssembly(assembly);

// Explicitly register the flight handlers from the Application project
builder.Services.AddTransient<MediatR.IRequestHandler<ABS.Notification.Application.Command.SendNotificationCommand, ABS.Notification.Application.Command.SendNotificationResult>, ABS.Notification.Application.Command.NotificationCommandHandler>();

// Application Services
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// MassTransit Configuration
builder.Services.AddMassTransit(config =>
{
	config.AddConsumer<PaymentProcessedConsumer>();
	config.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
		cfg.ReceiveEndpoint(EventBusConstants.PaymentProcessedQueue, c =>
		{
			c.ConfigureConsumer<PaymentProcessedConsumer>(context);
		});
	});
});

// Cross-Cutting Services
// 1. Register Health Check Services
builder.Services.AddHealthChecks()
		.AddSqlServer(
				connectionString: connectionString,
				healthQuery: "SELECT 1;", // Default, but can be customized
				name: "sql-server",
				tags: new[] { "ready" }
		);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseHealthChecks("/health",
	new HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();