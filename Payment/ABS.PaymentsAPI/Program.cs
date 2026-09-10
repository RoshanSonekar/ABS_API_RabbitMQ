using ABS.Payments.Core.Repositories;
using ABS.Payments.Infrastructure.Repositories;
using ABS.Payments.Application;
using BuildingBlocks.Behaviors;
using FluentValidation;
using HealthChecks.UI.Client;
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
builder.Services.AddTransient<MediatR.IRequestHandler<ABS.Payments.Application.Command.ProcessPaymentCommand, ABS.Payments.Application.Command.ProcessPaymentResult>, ABS.Payments.Application.Command.PaymentCommandHandler>();

// Application Services
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

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