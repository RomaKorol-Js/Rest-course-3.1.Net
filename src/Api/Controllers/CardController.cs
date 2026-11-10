using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Cards.Commands;
using MediatR;
using Domain.Cards;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("cards")]
[ApiController]
public class CardsController(ISender sender, ICardQueries cardQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CardDto>>> GetCards(CancellationToken cancellationToken)
    {
        var cards = await cardQueries.GetAll(cancellationToken);
        return cards.Select(CardDto.FromDomainModel).ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CardDto>> GetCard(Guid id, CancellationToken cancellationToken)
    {
        var cardOption = await cardQueries.GetById(new CardId(id), cancellationToken);

        return cardOption.Match<ActionResult<CardDto>>(
            c => CardDto.FromDomainModel(c!),
            () => NotFound()
        );
    }

    [HttpPost]
    public async Task<ActionResult<CardDto>> CreateCard([FromBody] CreateCardDto request, CancellationToken cancellationToken)
    {
        var input = new CreateCardCommand
        {
            Name = request.Name,
            Description = request.Description,
            Attack = request.Attack,
            Cost = request.Cost,
            Health = request.Health
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CardDto>>(
            p => CreatedAtAction(nameof(GetCard), new { id = p.Id.Value }, CardDto.FromDomainModel(p)),
            e => e.ToObjectResult()
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CardDto>> UpdateCard(Guid id, [FromBody] UpdateCardDto request, CancellationToken cancellationToken)
    {
        var input = new UpdateCardCommand
        {
            Name = request.Name,
            Description = request.Description,
            Attack = request.Attack,
            Cost = request.Cost,
            Health = request.Health
        };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CardDto>>(p => Ok(CardDto.FromDomainModel(p)),e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCard(Guid id, CancellationToken cancellationToken)
    {
        var input = new DeleteCardCommand { Id = id };

        var result = await sender.Send(input, cancellationToken);

        return result.Match<IActionResult>(p => NoContent(),e => e.ToObjectResult());
    }
}