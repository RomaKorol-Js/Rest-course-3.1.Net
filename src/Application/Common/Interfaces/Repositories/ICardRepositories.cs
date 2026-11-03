using Domain.Cards;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface ICardRepository
{
	Task<IReadOnlyList<Card>> GetAll(CancellationToken cancellationToken);
	Task<Option<Card?>> GetById(CardId id, CancellationToken cancellationToken);
	Task<Option<Card?>> GetByName(string Name, CancellationToken cancellationtoken);
    Task<Card> Add(Card card, CancellationToken cancellationToken);
	Task<Card> Update(Card card, CancellationToken cancellationToken);
	Task Delete(Card card, CancellationToken cancellationToken);
}