namespace ABS.Payments.Core.Entities;
public class PaymentEntity
{
	public Guid Id { get; set; }
	public Guid BookingId { get; set; } = Guid.Empty;
	public decimal Amount { get; set; } = 0.0m;
	//public string Currency { get; set; } = string.Empty;
	public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
	//public string PaymentMethod { get; set; } = string.Empty; // e.g., Credit Card, PayPal, etc.
}
