using App;

Deck deck = new();
List<Player> playersList = new();

bool isRunning = true;

while (isRunning)
{
  TryClear();

  Console.WriteLine("\nFLIP 7\n");
  Console.WriteLine("\nHow many players (min. 3, max. 10)?");
  Console.Write("▶ ");

  if (int.TryParse(Console.ReadLine(), out int playersNumb) && playersNumb > 2 && playersNumb <= 10)
  {
    TryClear();
    Console.WriteLine("\nInsert players names\n");
    for (int i = 0; i < playersNumb; ++i)
    {
      bool creating = true;
      while (creating)
      {
        Console.Write($"\nPlayer {i + 1} name? ");
        string? newPlayerName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPlayerName))
        { playersList.Add(CreatePlayer(newPlayerName)); creating = false; }
        else { Console.Write("\nPlayer name can't be empty. "); Console.ReadLine(); }
      }
    }
  }
  else if (playersNumb < 3)
  {
    Console.Write("\nYou need at least 3 players to play. "); Console.ReadLine();
  }
  else if (playersNumb > 10)
  {
    Console.Write("\nNo more than 10 players can play. "); Console.ReadLine();
  }
  else
  {
    Console.Write("\nInvalid input. "); Console.ReadLine();
  }

  TryClear();
  Console.WriteLine("\nLet's play!\n");
  for (int i = 0; i < playersList.Count; ++i)
  {
    Console.WriteLine($"{playersList[i].Name}");
  }
  Console.Write("\nPress ENTER when ready to play. ");
  Console.ReadLine();

  bool inGame = true;

  while (inGame)
  {

    foreach (Player player in playersList)
    {
      bool playing = true;
      while (playing)
      {
        TryClear();
        int roundCount = 1;
        Console.WriteLine($"\nRound {roundCount}.\n");
        Console.WriteLine($"\n{player.Name}'s turn.\n");
        Console.WriteLine("\nYour cards:");
        foreach (Card normalCard in player.PlayerCards)
        {
          if (normalCard.CardType == CardType.Normal)
          { Console.Write($" {normalCard.CardInfo()}"); }
        }
        Console.WriteLine("");
        foreach (Card modifierCard in player.PlayerCards)
        {
          if (modifierCard.CardType == CardType.Modifier)
          { Console.Write($" {modifierCard.CardInfo()}"); }
        }
        Console.WriteLine("");
        foreach (Card actionCard in player.PlayerCards)
        {
          if (actionCard.CardType == CardType.Action)
          { Console.Write($" {actionCard.CardInfo()}"); }
        }
        Console.WriteLine("");
        Console.Write("\n[D]raw | [S]tay: ");
        switch (Console.ReadLine()?.ToLower())
        {
          case "d":
            Card? drawnCard = deck.CardList[RandomCard()];
            Console.WriteLine($"\n {drawnCard.CardInfo()} \n");
            deck.CardList.Remove(drawnCard);
            foreach (Card card in player.PlayerCards)
            {
              if (drawnCard.CardType == CardType.Normal && card.Value == drawnCard.Value)
              {
                Console.WriteLine("\nB U S T E D!");
                Console.ReadLine();
                playing = false;
                break;
              }
            }
            if (playing)
            {
              player.PlayerCards.Add(drawnCard);
            }
            break;
          case "s":
            playing = false;
            break;
          default:
            break;
        }
      }
    }
  }
}


/* TEST CODE
 Card drawnCard = deck.CardList[RandomCard()];
Console.WriteLine($"{drawnCard.CardInfo()}");
Console.ReadLine(); */


// Method to draw a random card.
void TryClear()
{
  try { Console.Clear(); } catch { }
}
Player CreatePlayer(string name)
{
  return new(name, 0);
}
int RandomCard()
{
  Random rnd = new();

  return rnd.Next(deck.CardList.Count);
}
