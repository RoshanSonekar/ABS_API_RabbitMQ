using ABS.Payments.Core.Entities;

namespace ABS.Payments.Core.Repositories;

public interface IPaymentRepository
{
	Task ProcessPaymentAsync(PaymentEntity payment);
	Task RefundPaymentAsync(Guid id);
}
