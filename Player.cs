namespace App;

class Player
{
  public string Name;
  public int Score;
  public List<Card> PlayerCards = new();
  public bool Active;

  public Player(string name, int score)
  {
    Name = name;
    Score = score;
    Active = true;
  }

  public string PlayerInfo()
  {
    return $"Player: {Name} || Score: {Score}";
  }

  public int CountScore()
  {
    int score = 0;
    foreach (Card card in PlayerCards)
    {
      if (card.CardType == CardType.Normal)
      {
        score += card.Value;
      }
    }
    foreach (Card card in PlayerCards)
    {
      if (card.CardType == CardType.Double)
      {
        score *= 2;
      }
    }
    foreach (Card card in PlayerCards)
    {
      if (card.CardType == CardType.Modifier)
      {
        score += card.Value;
      }
    }
    return score;
  }
}