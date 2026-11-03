using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Cards;
using Microsoft.EntityFrameworkCore;
using LanguageExt;

namespace Infrastructure.Persistence.Repositories;

public class CardRepository(ApplicationDbContext context) :
ICardRepository, ICardQueries
{
    public async Task<IReadOnlyList<Card>> GetAll(CancellationToken
   cancellationToken)
    {
        return await context.Cards.ToListAsync(cancellationToken);
    }
    public async Task<Option<Card?>> GetById(CardId id, CancellationToken
   cancellationToken)
    {
        var entity = await context.Cards.FirstOrDefaultAsync(x => x.Id == id,
cancellationToken);
        return entity ?? Option<Card>.None;
    }
    public async Task<Card> Add(Card card, CancellationToken
   cancellationToken)
    {
        await context.Cards.AddAsync(card, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return card;
    }

    public async Task<Option<Card?>> GetByName(string Name, CancellationToken cancellationToken)
    {
        var entity = await context.Cards.FirstOrDefaultAsync(x => x.Name == Name, cancellationToken);
        return entity ?? Option<Card>.None;
    }
    public async Task<Card> Update(Card card, CancellationToken cancellationToken)
    {
        context.Cards.Update(card);
        //await card.UpdateAsync(card, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return card;

    }
    public async Task Delete(Card card, CancellationToken cancellationToken)
    {
        context.Cards.Remove(card);
        //await card.DeleteAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    // GetByTitle, Update, Delete: ��������� ��������� 
    // ϳ������: context.Cards.Update(...), context.Cards.Remove(...),
    //context.SaveChangesAsync(...)
}
//Application.Common.Interfaces Domain.Cards Microsoft.EntityFrameworkCore I