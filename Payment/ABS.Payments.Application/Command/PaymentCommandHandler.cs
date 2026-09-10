using ABS.Payments.Core.Entities;
using ABS.Payments.Core.Repositories;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace ABS.Payments.Application.Command;
public record ProcessPaymentResult(Guid Id, bool IsSuccess);
public record ProcessPaymentCommand(decimal Amount, Guid BookingId, DateTime PaymentDate) : ICommand<ProcessPaymentResult>;

public class PaymentCommandHandler(IPaymentRepository paymentRepository)
	: ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
{
	public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
	{
		var payment = new PaymentEntity
		{
			Id = Guid.NewGuid(),
			BookingId = command.BookingId,
			Amount = command.Amount,
			PaymentDate = DateTime.UtcNow
			//PaymentMethod = command.PaymentMethod
		};
		await paymentRepository.ProcessPaymentAsync(payment);
		return new ProcessPaymentResult(payment.Id, true);
	}
}