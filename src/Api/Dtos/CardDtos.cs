using System.ComponentModel.DataAnnotations;
using Domain.Card;

namespace Api.Dtos;

public record CardDto(
    Guid Id, string Name, string Description,int Attack, int Health, int Cost, DateTime CreatedAt
){
    public static CardDto FromDomainModel(Card card) 
    => new(card.Id,card.Name, card.Description, card.Attack, card.Health, card.Cost,card.CreatedAt);
}
public record CreateCardDto(
    [Required, MaxLength(255)] string Name,
    [Required, MaxLength(512)] string Description,
    [Required, Range(0, 255)] int Attack,
    [Required, Range(0, 255)] int Health,
    [Required, Range(0, 255)] int Cost);

public record UpdateCardDto(
    [Required, MaxLength(255)] string Name,
    [MaxLength(512)] string Description,
    [Range(0, 255)] int Attack,
    [Range(0, 255)] int Health,
    [Range(0, 255)] int Cost);