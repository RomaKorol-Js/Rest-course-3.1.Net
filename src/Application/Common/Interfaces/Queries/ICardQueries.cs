
using Domain.Cards;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface ICardQueries
{
    Task<IReadOnlyList<Card>> GetAll(CancellationToken cancellationToken);
    Task<Option<Card?>> GetById(CardId id, CancellationToken cancellationToken);
    Task<Option<Card?>> GetByName(string name, CancellationToken cancellationToken);
}