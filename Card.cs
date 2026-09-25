using System.Drawing;

namespace App;

class Card
{
    public string Name;
    public int Value;
    public CardType CardType;
    public ConsoleColor FrontColor;
    public ConsoleColor BackColor;
    public Card(string name, int value, CardType cardType, ConsoleColor frontColor, ConsoleColor backColor)
    {
        Name = name;
        Value = value;
        CardType = cardType;
        FrontColor = frontColor;
        BackColor = backColor;
    }
    public string CardInfo()
    {
        if (CardType == CardType.Normal)
        {
            return $"| {Value} |";

        }
        else
        {
            return $"| {Name} |";
        }
    }
}

