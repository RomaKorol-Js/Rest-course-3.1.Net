using Application.Common.Interfaces.Repositories;
using Application.Cards.Exceptions;
using Domain.Cards;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Cards.Commands;

public record UpdateCardCommand : IRequest<Either<CardException, Card>>
{
    public required CardId Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int Attack { get; init; }
    public required int Cost { get; init; }
    public required int Health { get; init; }
}

public class UpdateCardCommandHandler(ICardRepository cardRepository) : IRequestHandler<UpdateCardCommand, Either<CardException, Card>>
{
    public async Task<Either<CardException, Card>> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var cardOption = await cardRepository.GetById(request.Id, cancellationToken);

        return await cardOption.MatchAsync(
            async c => await CheckDuplicates(c!.Id, request.Name, cancellationToken)
                .BindAsync(_ => UpdateEntity(c!, request, cancellationToken)),
            () => Task.FromResult<Either<CardException, Card>>(new CardNotFoundException(request.Id))
        );
    }

    private async Task<Either<CardException, Unit>> CheckDuplicates(CardId currentCardId, string name, CancellationToken cancellationToken)
    {
        var cardOption = await cardRepository.GetByName(name, cancellationToken);
        return cardOption.Match<Either<CardException, Unit>>(
            c => c!.Id == currentCardId ? Unit.Default : new CardAlreadyExistsException(c!.Id),
            () => Unit.Default
        );
    }

    private async Task<Either<CardException, Card>> UpdateEntity(Card card, UpdateCardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            card.UpdateDetails(request.Name, request.Description, request.Cost, request.Attack, request.Health);
            return await cardRepository.Update(card, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CardUnhandledException(card.Id, exception);
        }
    }
}