using Application.Common.Interfaces;
using Application.Cards.Services.Abstract;
using Domain.Card;

namespace Application.Cards.Services.Implementation;

public class CardService(ICardRepository cardRepository): ICardService
{

    public async Task<Card> Add(string name, string description, int cost,int attack,int health,CancellationToken cancellationToken){
    var existingCard = await cardRepository.GetByName(name, cancellationToken);
    if(existingCard is not null)
    {
        throw new ArgumentException($"Card with name '{name}' already exist ");
    }

    var card = Card.New(Guid.NewGuid(),name,description,cost,attack,health);
    return await cardRepository.Add(card, cancellationToken);
    }
    public async Task<Card?> Update(Guid id,string name, string description, int cost,int attack ,int health, CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetById(id, cancellationToken);
        if(card is null){
            return null;
        }

        var cardWithSameName = await cardRepository.GetByName(name,cancellationToken);
        if (cardWithSameName is not null && cardWithSameName.Id != id)
        {
            throw new ArgumentException($"Card with name '{name}' already exist");
        }
        card.UpdateDetails(name,description,cost,attack,health);
        return await cardRepository.Update(card, cancellationToken);
    }
    public async Task<IReadOnlyList<Card>> GetCards(CancellationToken cancellationToken)
    {
        return await cardRepository.GetAll(cancellationToken);
    }
    public async Task<Card?> GetCard(Guid id,CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetById(id,cancellationToken);
        if(card is null){
            return null;
        }
        return card;
    }
    public async Task<bool> Delete(Guid id,CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetById(id,cancellationToken);
        if(card is null){
            return false;
        }
        await cardRepository.Delete(card, cancellationToken);
        return true;
    }//this can be a mistake bc idk if its work right that I wrote
}