using Domain.Card;

namespace Application.Cards.Services.Abstract;

public interface ICardService
{
    Task<IReadOnlyList<Card>> GetCards(CancellationToken cancellationToken);
    Task<Card?> GetCard(Guid id, CancellationToken cancellationToken);
    Task<Card> Add(string name,string description, int cost,int attack,int health, CancellationToken cancellationToken);
    Task<Card?> Update(Guid id,string name, string description, int cost, int attack, int health, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}

