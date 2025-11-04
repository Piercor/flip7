using System.Data;
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

  bool scoreBoard = true;
  bool inGame = true;
  int roundCount = 0;

  while (inGame)
  {
    bool round = true;

    roundCount++;

    foreach (Player player in playersList)
    {
      if (player.PlayerCards.Count > 0)
      {
        foreach (Card card in player.PlayerCards)
        {
          deck.DiscardList.Add(card);
        }
      }
      player.PlayerCards.Clear();
    }

    if (roundCount > 1)
    {
      Console.WriteLine($"\nScores after round {roundCount - 1}.\n");
      foreach (Player player1 in playersList)
      {
        Console.WriteLine(player1.PlayerInfo());
      }
      Console.Write("\nPress ENTER to start next round. ");
      Console.ReadLine();
    }

    /* TEST CODE
    foreach (Card discard in deck.DiscardList)
    { Console.WriteLine(discard.CardInfo()); }
    Console.Write("\n>>>");
    Console.ReadLine(); */

    while (round)
    {
      foreach (Player player in playersList)
      {
        bool playing = false;
        if (player.Active == true)
        {
          playing = true;
        }
        while (playing)
        {
          TryClear();
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
            if (modifierCard.CardType == CardType.Modifier || modifierCard.CardType == CardType.Double)
            { Console.Write($" {modifierCard.CardInfo()}"); }
          }
          Console.WriteLine("");
          foreach (Card actionCard in player.PlayerCards)
          {
            /* When Second Chance, Freeze and Flip Three have functionality, here would only be
            "CardType.SecondChance", because FlipThree and Freeze can't be stored */
            if (actionCard.CardType == CardType.SecondChance || actionCard.CardType == CardType.Freeze || actionCard.CardType == CardType.FlipThree)
            { Console.Write($" {actionCard.CardInfo()}"); }
          }
          Console.WriteLine("");
          Console.WriteLine("\n[D]raw | [S]tay | [T]oggle score board. ");

          if (scoreBoard)
          {
            Console.WriteLine("");
            Console.WriteLine("\nSCORE BOARD");
            foreach (Player player1 in playersList)
            {
              Console.WriteLine($"{player1.Name}: {player1.Score}");
            }
          }
          Console.Write("\n▶ ");

          switch (Console.ReadKey().Key)
          {
            case ConsoleKey.D:
              Card? drawnCard = deck.CardList[RandomCard()];
              Console.WriteLine($"");
              Console.WriteLine($"\nDrawn card:");
              Console.WriteLine($"\n {drawnCard.CardInfo()}");
              deck.CardList.Remove(drawnCard);
              int normalCardCount = 0;
              foreach (Card card in player.PlayerCards)
              {
                if (drawnCard.CardType == CardType.Normal && card.CardType == CardType.Normal && card.Value == drawnCard.Value)
                {
                  Console.WriteLine("\nB U S T E D !");
                  Console.Write("\nPress ENTER to continue. ");
                  Console.ReadLine();
                  player.PlayerCards.Add(drawnCard);
                  player.Active = false;
                  playing = false;
                  break;
                }
                if (drawnCard.CardType == CardType.Normal)
                {
                  normalCardCount++;
                }
              }

              if (playing)
              {
                player.PlayerCards.Add(drawnCard);
                if (normalCardCount == 7)
                {
                  player.Score += player.CountScore();
                  player.Score += 15;
                  Console.WriteLine("\n F L I P  7 !");
                  Console.WriteLine($"\nYour score this round is {player.CountScore()}");
                  Console.WriteLine($"Your total score is {player.Score}");
                  Console.Write("\nPress ENTER to continue. ");
                  Console.ReadLine();
                  player.Active = false;
                }
                playing = false;
              }
              break;
            case ConsoleKey.S:
              player.Score += player.CountScore();
              playing = false;
              player.Active = false;
              Console.WriteLine($"\nYour score this round is {player.CountScore()}");
              Console.WriteLine($"Your total score is {player.Score}");
              Console.Write("\nPress ENTER to continue. ");
              Console.ReadLine();
              break;

            case ConsoleKey.T:
              if (!scoreBoard) { scoreBoard = true; }
              else { scoreBoard = false; }
              break;
          }
        }
        if (player.Active)
        {
          Console.WriteLine("\n[C]ontinue or [S]tay?");
          Console.Write("\n▶ ");
          switch (Console.ReadKey().Key)
          {
            case ConsoleKey.C: continue;
            case ConsoleKey.S:
              player.Score += player.CountScore();
              playing = false;
              player.Active = false;
              Console.WriteLine($"\nYour score this round is {player.CountScore()}");
              Console.WriteLine($"Your total score is {player.Score}");
              Console.Write("\nPress ENTER to continue. ");
              Console.ReadLine();
              continue;
          }
        }
      }
      bool activePlayers = false;
      foreach (Player player in playersList)
      {
        if (player.Active == true)
        {
          activePlayers = true;
        }
      }
      if (!activePlayers)
      {
        round = false;
      }
    }
    foreach (Player player in playersList)
    {
      player.Active = true;
    }
    continue;
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
