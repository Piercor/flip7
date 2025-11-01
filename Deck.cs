using System.Diagnostics;

namespace App;

class Deck
{
  public List<Card> CardList = new();

  public Deck()
  {
    Debug.Assert(CardList != null);
    for (int i = 0; i < 12; ++i)
    {
      CardList.Add(new("Twelve", 12, CardType.Normal));
    }
    for (int i = 0; i < 11; ++i)
    {
      CardList.Add(new("Eleven", 11, CardType.Normal));
    }
    for (int i = 0; i < 10; ++i)
    {
      CardList.Add(new("Ten", 10, CardType.Normal));
    }
    for (int i = 0; i < 9; ++i)
    {
      CardList.Add(new("Nine", 9, CardType.Normal));
    }
    for (int i = 0; i < 8; ++i)
    {
      CardList.Add(new("Eight", 8, CardType.Normal));
    }
    for (int i = 0; i < 7; ++i)
    {
      CardList.Add(new("Seven", 7, CardType.Normal));
    }
    for (int i = 0; i < 6; ++i)
    {
      CardList.Add(new("Six", 6, CardType.Normal));
    }
    for (int i = 0; i < 5; ++i)
    {
      CardList.Add(new("Five", 5, CardType.Normal));
    }
    for (int i = 0; i < 4; ++i)
    {
      CardList.Add(new("Four", 4, CardType.Normal));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardList.Add(new("Three", 3, CardType.Normal));
    }
    for (int i = 0; i < 2; ++i)
    {
      CardList.Add(new("Two", 2, CardType.Normal));
    }
    CardList.Add(new("One", 1, CardType.Normal));
    CardList.Add(new("Zero", 0, CardType.Normal));

    CardList.Add(new("+2", 2, CardType.Modifier));
    CardList.Add(new("+4", 4, CardType.Modifier));
    CardList.Add(new("+6", 6, CardType.Modifier));
    CardList.Add(new("+8", 8, CardType.Modifier));
    CardList.Add(new("+10", 10, CardType.Modifier));

    for (int i = 0; i < 3; ++i)
    {
      CardList.Add(new("Freeze", 0, CardType.Action));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardList.Add(new("Flip Three", 0, CardType.Action));
    }
    for (int i = 0; i < 3; ++i)
    {
      CardList.Add(new("Second Chance", 0, CardType.Action));
    }
  }
}