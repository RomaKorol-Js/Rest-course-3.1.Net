using Application.Common.Interfaces.Repositories;
using Application.Cards.Exceptions;
using Domain.Cards;
using LanguageExt;
using MediatR;

namespace Application.Cards.Commands;

public record DeleteCardCommand : IRequest<Either<CardException, Card>>
{
    public required Guid Id { get; init; }
}

public class DeleteCardCommandHandler(ICardRepository cardRepository) : IRequestHandler<DeleteCardCommand, Either<CardException, Card>>
{
    public async Task<Either<CardException, Card>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var cardId = new CardId(request.Id);

        var cardOption = await cardRepository.GetById(cardId, cancellationToken);

        return await cardOption.MatchAsync(
            async card =>
            {
                await cardRepository.Delete(card!, cancellationToken);
                Either<CardException, Card> successResult = card!;
                return successResult;
            },
            () => Task.FromResult<Either<CardException, Card>>(new CardNotFoundException(cardId))
        );
    }
}