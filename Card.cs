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

}

