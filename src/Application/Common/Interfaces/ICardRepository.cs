using Domain.Card;

namespace Application.Common.Interfaces;

public interface ICardRepository
{
    Task<IReadOnlyList<Card>> GetAll(CancellationToken cancellationToken);
    Task<Card?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Card?> GetByName(string Name, CancellationToken cancellationtoken);
    Task<Card> Add(Card card, CancellationToken cancellationToken);
    Task<Card> Update(Card card, CancellationToken cancellationToken);
    Task Delete(Card card, CancellationToken cancellationToken);
}