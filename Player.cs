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

  public bool CheckBusted(Card drawnCard)
  {
    bool busted = false;
    foreach (Card card in PlayerCards)
    {
      if (drawnCard.CardType == CardType.Normal && card.CardType == CardType.Normal && card.Value == drawnCard.Value)
      {
        busted = true;
      }
    }
    if (busted) { return true; } else { return false; }
  }
  public bool CheckFlip7()
  {
    int normalCardCount = 0;
    foreach (Card card in PlayerCards)
    {
      if (card.CardType == CardType.Normal)
      {
        normalCardCount++;
      }
    }
    if (normalCardCount == 7)
    { return true; }
    else { return false; }
  }
  public void EmptyPlayerCards(Deck deck)
  {
    if (PlayerCards.Count > 0)
    {
      foreach (Card card in PlayerCards)
      {
        deck.DiscardPile.Add(card);
      }
    }
    PlayerCards.Clear();
  }
  // Method to show cards.
  public void ShowCards()
  {
    for (int tries = 0; tries < PlayerCards.Count; ++tries)
    {
      for (int i = 1; i < PlayerCards.Count; ++i)
      {
        Card? current = PlayerCards[i];
        Card? prev = PlayerCards[i - 1];
        if (current.Value > prev.Value)
        {
          PlayerCards[i] = prev;
          PlayerCards[i - 1] = current;
        }
      }
    }
    foreach (Card normalCard in PlayerCards)
    {
      if (normalCard.CardType == CardType.Normal)
      {

        Console.Write($" ");
        Console.BackgroundColor = normalCard.BackColor;
        Console.ForegroundColor = normalCard.FrontColor;
        Console.Write($"{normalCard.CardInfo()}");
        Console.ResetColor();
      }
    }
    Console.WriteLine("");
    foreach (Card modifierCard in PlayerCards)
    {
      if (modifierCard.CardType == CardType.Modifier || modifierCard.CardType == CardType.Double)
      {
        Console.Write($" ");
        Console.BackgroundColor = modifierCard.BackColor;
        Console.ForegroundColor = modifierCard.FrontColor;
        Console.Write($"{modifierCard.CardInfo()}");
        Console.ResetColor();
      }
    }
    Console.WriteLine("");
    foreach (Card actionCard in PlayerCards)
    {
      if (actionCard.CardType == CardType.SecondChance)
      {
        Console.Write($" ");
        Console.BackgroundColor = actionCard.BackColor;
        Console.ForegroundColor = actionCard.FrontColor;
        Console.Write($"{actionCard.CardInfo()}");
        Console.ResetColor();
      }
    }
    Console.WriteLine("");
  }
}