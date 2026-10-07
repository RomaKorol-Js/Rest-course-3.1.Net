using Api.Dtos;
using Application.Cards.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers;

[Route("cards")]
[ApiController]
public class CardController(ICardService cardService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCards(CancellationToken cancellationToken)
    {
        var cards = await cardService.GetCards(cancellationToken);

        var dtos = cards.Select(card => CardDto.FromDomainModel(card));
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCard(Guid id, CancellationToken cancellationToken)
    {
        var card = await cardService.GetCard(id, cancellationToken);
        if (card is null)
        {
            return NotFound();
        }

        return Ok(CardDto.FromDomainModel(card));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardDto request, CancellationToken cancellationToken)
    {
        try
        {
            var card = await cardService.Add(request.Name, request.Description, request.Cost, request.Attack, request.Health, cancellationToken);
            var dto = CardDto.FromDomainModel(card);

            return CreatedAtAction(nameof(GetCard), new { id = dto.Id }, dto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCard(Guid id, [FromBody] UpdateCardDto request, CancellationToken cancellationToken)
    {
        try
        {
            var card = await cardService.Update(id, request.Name, request.Description, request.Cost, request.Attack, request.Health, cancellationToken);
            if (card is null)
            {
                return NotFound();
            }

            return Ok(CardDto.FromDomainModel(card));
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCard(Guid id, CancellationToken cancellationToken)
    {
        var success = await cardService.Delete(id, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}