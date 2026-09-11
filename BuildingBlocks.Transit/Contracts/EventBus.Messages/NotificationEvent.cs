namespace BuildingBlocks.Transit.Contracts.EventBus.Messages;
public record NotificationEvent(string Recipient, string Message, string Type);
