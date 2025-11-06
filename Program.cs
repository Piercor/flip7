using System.Data;
using App;

Deck deck = new();
List<Player> playersList = new();

int pointsToReach = 200;
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

  bool inGame = false;
  if (playersList.Count >= 3 && playersList.Count <= 10)
  {
    inGame = true;
    TryClear();
    Console.WriteLine("\nPoints to reach? (200 by default)?");
    Console.Write("▶ ");
    if (int.TryParse(Console.ReadLine(), out int userPoints) && playersNumb > 2 && playersNumb <= 10)
    {
      pointsToReach = userPoints;
    }

    TryClear();
    Console.WriteLine("\nLet's play!\n");
    for (int i = 0; i < playersList.Count; ++i)
    {
      Console.WriteLine($"{playersList[i].Name}");
    }
    Console.WriteLine($"\nPoints to reach: {pointsToReach}.");
    Console.Write("\nPress ENTER when ready to play. ");
    Console.ReadLine();
  }

  bool scoreBoard = true;
  int roundCount = 0;

  while (inGame)
  {
    bool round = true;
    roundCount++;

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
          if (scoreBoard)
          {
            Console.WriteLine("\n------------------------------");
            Console.WriteLine("\nSCORE BOARD\n");
            foreach (Player player1 in playersList)
            {
              Console.WriteLine($"{player1.Name}: {player1.Score}");
            }
            Console.WriteLine("\n------------------------------");
          }

          Console.WriteLine("");
          Console.Write("\n[D]raw | [S]tay | [T]oggle score board. ");

          switch (Console.ReadKey().Key)
          {
            case ConsoleKey.D:
              Card? drawnCard = deck.CardDeck[RandomCard()];
              Console.WriteLine($"");
              Console.WriteLine($"\nDrawn card:");
              Console.WriteLine($"\n {drawnCard.CardInfo()}");
              deck.CardDeck.Remove(drawnCard);

              switch (drawnCard.CardType)
              {
                case CardType.Modifier: player.PlayerCards.Add(drawnCard); playing = false; break;
                case CardType.Double: player.PlayerCards.Add(drawnCard); playing = false; break;
                case CardType.SecondChance: player.PlayerCards.Add(drawnCard); playing = false; break;
                case CardType.Normal:
                  if (player.CheckBusted(drawnCard))
                  {
                    bool foundSecondChance = false;
                    foreach (Card secondChanceCard in player.PlayerCards)
                    {
                      if (secondChanceCard.CardType == CardType.SecondChance)
                      {
                        foundSecondChance = true;
                        player.PlayerCards.Remove(secondChanceCard);
                        deck.DiscardPile.Add(secondChanceCard);
                        break;
                      }
                    }
                    if (!foundSecondChance)
                    {
                      Console.WriteLine("\nB U S T E D !");
                      Console.Write("\nPress ENTER to continue. ");
                      Console.ReadLine();
                      player.PlayerCards.Add(drawnCard);
                      player.EmptyPlayerCards(deck);
                      player.Active = false;
                      playing = false;
                      break;
                    }
                    else
                    {
                      deck.DiscardPile.Add(drawnCard);
                      playing = false;
                      Console.WriteLine("\nThat was close, luckily you had a Second Chance to save you!");
                      Console.Write("\nPress ENTER to continue. ");
                      Console.ReadLine();
                    }
                  }
                  if (playing)
                  {
                    player.PlayerCards.Add(drawnCard);
                    if (player.CheckFlip7())
                    {
                      player.Score += player.CountScore();
                      player.Score += 15;
                      Console.WriteLine("\n F L I P  7 !");
                      Console.WriteLine($"\nYour score this round is {player.CountScore() + 15}");
                      Console.WriteLine($"Your total score is {player.Score}");
                      Console.Write("\nPress ENTER to continue. ");
                      Console.ReadLine();
                      player.EmptyPlayerCards(deck);
                      player.Active = false;
                    }
                    playing = false;
                  }
                  break;

                case CardType.Freeze:
                  deck.DiscardPile.Add(drawnCard);
                  Console.WriteLine("\nW I P"); playing = false; break;
                case CardType.FlipThree:
                  deck.DiscardPile.Add(drawnCard);
                  Console.WriteLine("\nW I P"); playing = false; break;
              }

              break;
            case ConsoleKey.S:
              player.Score += player.CountScore();
              playing = false;
              player.Active = false;
              Console.WriteLine("");
              Console.WriteLine($"\nYour score this round is {player.CountScore()}");
              Console.WriteLine($"Your total score is {player.Score}");
              Console.Write("\nPress ENTER to continue. ");
              Console.ReadLine();
              player.EmptyPlayerCards(deck);
              break;

            case ConsoleKey.T:
              if (!scoreBoard) { scoreBoard = true; }
              else { scoreBoard = false; }
              break;
          }
        }
        if (player.Active)
        {
          Console.Write("\nPress [S] to stay or any other key to continue playing. ");

          switch (Console.ReadKey().Key)
          {
            case ConsoleKey.C: continue;
            case ConsoleKey.S:
              player.Score += player.CountScore();
              playing = false;
              player.Active = false;
              Console.WriteLine("");
              Console.WriteLine($"\nYour score this round is {player.CountScore()}");
              Console.WriteLine($"Your total score is {player.Score}");
              Console.Write("\nPress ENTER to continue. ");
              Console.ReadLine();
              player.EmptyPlayerCards(deck);
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

    bool winnerFound = false;
    foreach (Player player in playersList)
    {
      player.Active = true;
      if (player.Score >= pointsToReach)
      {
        winnerFound = true;
      }
    }

    if (winnerFound)
    {
      for (int tries = 0; tries < playersList.Count; ++tries)
      {
        for (int i = 1; i < playersList.Count; ++i)
        {
          Player? current = playersList[i];
          Player? prev = playersList[i - 1];
          if (current.Score > prev.Score)
          {
            playersList[i] = prev;
            playersList[i - 1] = current;
          }
        }
      }
      TryClear();
      if (playersList[0].Score == playersList[1].Score)
      {
        Console.WriteLine("\nWe have a tie!");
      }
      else
      {
        Console.WriteLine("\nWe have a winner!");
        Console.WriteLine($"\nContratulations, {playersList[0].Name}!");
      }

      Console.WriteLine($"\nFinal score board: \n");
      for (int i = 0; i < playersList.Count; ++i)
      {
        Console.WriteLine($"[{i + 1}] {playersList[i].PlayerInfo()}");
      }
      Console.WriteLine("\nThanks for playing!");
      Console.Write("\nPress ENTER to finish. ");
      Console.ReadLine();
      inGame = false;
      isRunning = false;
      break;
    }
    TryClear();
    Console.WriteLine($"\nScores after round {roundCount}.\n");
    foreach (Player player in playersList)
    {
      Console.WriteLine(player.PlayerInfo());
    }
    Console.Write("\nPress ENTER to start next round. ");
    Console.ReadLine();

    continue;

  }
}

// Method to clear console.
void TryClear()
{
  try { Console.Clear(); } catch { }
}
Player CreatePlayer(string name)
{
  return new(name, 0);
}
// Method to draw a random card.
int RandomCard()
{
  Random rnd = new();

  return rnd.Next(deck.CardDeck.Count);
}
