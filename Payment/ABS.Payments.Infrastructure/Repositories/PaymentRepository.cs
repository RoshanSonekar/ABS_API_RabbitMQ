using ABS.Payments.Core.Entities;
using ABS.Payments.Core.Repositories;
using Dapper;
using System.Data;

namespace ABS.Payments.Infrastructure.Repositories
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly IDbConnection _dbConnection;
		public PaymentRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}

		public async Task ProcessPaymentAsync(PaymentEntity payment)
		{
			const string sql = @"
				INSERT INTO Payments (Id, Amount, Currency, Status)
				VALUES (@Id, @Amount, @Currency, @Status)";

			await _dbConnection.ExecuteAsync(sql, payment);
		}

		public async Task RefundPaymentAsync(Guid id)
		{
			const string sql = "UPDATE Payments SET Status = 'Refunded' WHERE Id = @Id";
			await _dbConnection.ExecuteAsync(sql, new { Id = id });
		}
	}
}
