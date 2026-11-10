using Domain.Cards;

namespace Tests.Data.Cards;

public static class CardTestData
{
    public static Card FirstTestsCard() => Card.New(CardId.New(), "First test cards, Lish", "Card with balanced stats", 4, 4, 4);
    public static Card SecondTestsCard() => Card.New(CardId.New(), "Second test cards, Led", "Card with Hight ATK and HP", 6, 8, 7);
    public static Card ThirdTestsCard() => Card.New(CardId.New(), "Thired test cards, Kar", "Cheap week card", 2, 2, 1);

    public static Card InvalidTestsCard() => Card.New(CardId.New(), "Invalid test cards, %&%^&%", "Card with imposible stats", 6, 0, 4);
}