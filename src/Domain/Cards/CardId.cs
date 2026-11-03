namespace Domain.Cards;

public record CardId(Guid Value)
{
    public static CardId Empty() => new(Guid.Empty);
    public static CardId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}