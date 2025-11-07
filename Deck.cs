using System.Diagnostics;
using System.Drawing;

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
      CardDeck.Add(new("Twelve", 12, CardType.Normal, ConsoleColor.Gray, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 11; ++i)
    {
      CardDeck.Add(new("Eleven", 11, CardType.Normal, ConsoleColor.Blue, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 10; ++i)
    {
      CardDeck.Add(new("Ten", 10, CardType.Normal, ConsoleColor.DarkRed, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 9; ++i)
    {
      CardDeck.Add(new("Nine", 9, CardType.Normal, ConsoleColor.Yellow, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 8; ++i)
    {
      CardDeck.Add(new("Eight", 8, CardType.Normal, ConsoleColor.Green, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 7; ++i)
    {
      CardDeck.Add(new("Seven", 7, CardType.Normal, ConsoleColor.Red, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 6; ++i)
    {
      CardDeck.Add(new("Six", 6, CardType.Normal, ConsoleColor.Magenta, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 5; ++i)
    {
      CardDeck.Add(new("Five", 5, CardType.Normal, ConsoleColor.DarkGreen, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 4; ++i)
    {
      CardDeck.Add(new("Four", 4, CardType.Normal, ConsoleColor.Cyan, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Three", 3, CardType.Normal, ConsoleColor.Red, ConsoleColor.DarkGray));
    }
    for (int i = 0; i < 2; ++i)
    {
      CardDeck.Add(new("Two", 2, CardType.Normal, ConsoleColor.Green, ConsoleColor.DarkGray));
    }
    CardDeck.Add(new("One", 1, CardType.Normal, ConsoleColor.Gray, ConsoleColor.DarkGray));
    CardDeck.Add(new("Zero", 0, CardType.Normal, ConsoleColor.Black, ConsoleColor.DarkGray));

    CardDeck.Add(new("+2", 2, CardType.Modifier, ConsoleColor.DarkRed, ConsoleColor.DarkYellow));
    CardDeck.Add(new("+4", 4, CardType.Modifier, ConsoleColor.DarkRed, ConsoleColor.DarkYellow));
    CardDeck.Add(new("+6", 6, CardType.Modifier, ConsoleColor.DarkRed, ConsoleColor.DarkYellow));
    CardDeck.Add(new("+8", 8, CardType.Modifier, ConsoleColor.DarkRed, ConsoleColor.DarkYellow));
    CardDeck.Add(new("+10", 10, CardType.Modifier, ConsoleColor.DarkRed, ConsoleColor.DarkYellow));
    CardDeck.Add(new("x2", 0, CardType.Double, ConsoleColor.DarkRed, ConsoleColor.DarkYellow));

    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Freeze", 0, CardType.Freeze, ConsoleColor.White, ConsoleColor.Blue));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Flip Three", 0, CardType.FlipThree, ConsoleColor.White, ConsoleColor.DarkYellow));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardDeck.Add(new("Second Chance", 0, CardType.SecondChance, ConsoleColor.White, ConsoleColor.Red));
    }
  }
}