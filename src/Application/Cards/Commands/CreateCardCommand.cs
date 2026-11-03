using Application.Common.Interfaces.Repositories;
using Application.Cards.Exceptions;
using Domain.Cards;
using LanguageExt;
using MediatR;

namespace Application.Cards.Commands;

public record CreateCardCommand : IRequest<Either<CardException, Card>>
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int Attack {  get; init; }
    public required int Cost { get; init; }
    public required int Health { get; init; }
}

public class CreatCardCommandHandler(ICardRepository cardRepository): IRequestHandler<CreateCardCommand, Either<CardException,Card>>
{

    public async Task<Either<CardException,Card>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var card = await cardRepository.GetByName(request.Name, cancellationToken);


        return await card.MatchAsync(c => new CardAlreadyExistsException(c.Id), () => CreateEntity(request, cancellationToken));
        //var card = Card.New(Guid.NewGuid(), request.Name, request.Description, request.Cost, request.Attack, request.Health);
        //return await cardRepository.Add(card, cancellationToken);
    }
    public async Task<Either<CardException, Card>> CreateEntity(CreateCardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var card = Card.New(CardId.New(), request.Name, request.Description, request.Cost, request.Attack, request.Health);
            return await cardRepository.Add(card, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CardUnhandledException(CardId.Empty(), exception);
        }
    }
}