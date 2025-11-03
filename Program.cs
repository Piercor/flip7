using App;

Deck deck = new();

Card drawnCard = deck.CardList[RandomCard()];

Console.WriteLine($"{drawnCard.CardInfo()}");

drawnCard = deck.CardList[RandomCard()];

Console.WriteLine($"{drawnCard.CardInfo()}");

Console.ReadLine();


// Method to draw a random card.
int RandomCard()
{
  Random rnd = new();

  return rnd.Next(deck.CardList.Count);
}