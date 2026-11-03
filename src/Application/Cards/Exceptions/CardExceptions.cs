using Domain.Cards;

namespace Application.Cards.Exceptions;

public abstract class CardException(CardId cardId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public CardId CardId { get; } = cardId;
}

public class CardAlreadyExistsException(CardId cardId): CardException(cardId, $"Card already exists under id {cardId}");

public class CardNotFoundException(CardId cardId): CardException(cardId, $"Card not found under id {cardId}");

public class CardUnhandledException(CardId cardId, Exception? innerException = null): CardException(cardId, $"Unhandled exception under id {cardId}", innerException);