using App;

Deck newDeck = new();

foreach (Card card in newDeck.CardList)
{
  Console.WriteLine($"\n{card.CardInfo()}");
}

Console.ReadLine();