using ABS.Booking.Core.Repositories;
using ABS.Booking.Infrastructure.Repositories;
using BuildingBlocks.Behaviors;
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

// SQL Connection for Dapper
builder.Services.AddScoped<IDbConnection>(sp => 
new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application Services MediatR
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(assembly);
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));
	config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Explicitly register the GetBooking handler from the Application project
builder.Services.AddTransient<MediatR.IRequestHandler<ABS.Booking.Application.GetBooking.GetBookingQuery, ABS.Booking.Application.GetBooking.GetBookingResult>, ABS.Booking.Application.GetBooking.GetBookingQueryHandler>();
builder.Services.AddTransient<MediatR.IRequestHandler<ABS.Booking.Application.AddBooking.AddBookingCommand, ABS.Booking.Application.AddBooking.AddBookingResult>, ABS.Booking.Application.AddBooking.AddBookingCommandHandler>();

// Application Services
builder.Services.AddScoped<IBookingRepository, BookingRepository>();


// Cross-Cutting Services
// 1. Register Health Check Services
builder.Services.AddHealthChecks()
		.AddSqlServer(
				connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
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
