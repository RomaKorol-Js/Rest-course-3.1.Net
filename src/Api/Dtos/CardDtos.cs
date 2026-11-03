using System.ComponentModel.DataAnnotations;
using Domain.Cards;

namespace Api.Dtos;

public record CardDto(
    Guid Id, string Name, string Description,int Attack, int Health, int Cost){
    public static CardDto FromDomainModel(Card card) 
    => new(card.Id.Value,card.Name, card.Description, card.Attack, card.Health, card.Cost);
}
public record CreateCardDto(string Name,string Description,int Attack,int Health,int Cost);

public record UpdateCardDto(Guid Id,string Name,string Description,int Attack,int Health,int Cost);