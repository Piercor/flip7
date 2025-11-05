using System.Diagnostics;

namespace App;

class Deck
{
  public List<Card> CardDeck = new();
  public List<Card> DiscardPile = new();
  public Deck()
  {
    Debug.Assert(CardDeck != null);
    for (int i = 0; i < 12; ++i)
    {
      CardDeck.Add(new("Twelve", 12, CardType.Normal));
    }
    for (int i = 0; i < 11; ++i)
    {
      CardDeck.Add(new("Eleven", 11, CardType.Normal));
    }
    for (int i = 0; i < 10; ++i)
    {
      CardDeck.Add(new("Ten", 10, CardType.Normal));
    }
    for (int i = 0; i < 9; ++i)
    {
      CardDeck.Add(new("Nine", 9, CardType.Normal));
    }
    for (int i = 0; i < 8; ++i)
    {
      CardDeck.Add(new("Eight", 8, CardType.Normal));
    }
    for (int i = 0; i < 7; ++i)
    {
      CardDeck.Add(new("Seven", 7, CardType.Normal));
    }
    for (int i = 0; i < 6; ++i)
    {
      CardDeck.Add(new("Six", 6, CardType.Normal));
    }
    for (int i = 0; i < 5; ++i)
    {
      CardDeck.Add(new("Five", 5, CardType.Normal));
    }
    for (int i = 0; i < 4; ++i)
    {
      CardDeck.Add(new("Four", 4, CardType.Normal));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Three", 3, CardType.Normal));
    }
    for (int i = 0; i < 2; ++i)
    {
      CardDeck.Add(new("Two", 2, CardType.Normal));
    }
    CardDeck.Add(new("One", 1, CardType.Normal));
    CardDeck.Add(new("Zero", 0, CardType.Normal));

    CardDeck.Add(new("+2", 2, CardType.Modifier));
    CardDeck.Add(new("+4", 4, CardType.Modifier));
    CardDeck.Add(new("+6", 6, CardType.Modifier));
    CardDeck.Add(new("+8", 8, CardType.Modifier));
    CardDeck.Add(new("+10", 10, CardType.Modifier));
    CardDeck.Add(new("x2", 0, CardType.Double));

    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Freeze", 0, CardType.Freeze));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Flip Three", 0, CardType.FlipThree));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Second Chance", 0, CardType.SecondChance));
    }
  }
}