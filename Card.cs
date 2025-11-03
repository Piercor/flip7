namespace App;

class Card
{
  public string Name;
  public int Value;
  public CardType CardType;
  public Card(string name, int value, CardType cardType)
  {
    Name = name;
    Value = value;
    CardType = cardType;
  }
  public string CardInfo()
  {
    if (CardType == CardType.Action)
    {
      return $"{Name}";
    }
    else
    {
      return $"{Value}\n{Name}";
    }
  }
}

