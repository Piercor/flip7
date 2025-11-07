using System.Data;
using System.Diagnostics;
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
    int activePlayersCount = playersList.Count;
    while (round)
    {
      foreach (Player player in playersList)
      {
        /*  TryClear();
         if (player.Active && activePlayersCount > 1)
         {
           Console.WriteLine($"\nRound {roundCount}.\n");
           Console.WriteLine($"\n{player.Name}'s turn.\n");
           Thread.Sleep(500);
         } */
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
          if (player.Active && activePlayersCount > 1)
          { Thread.Sleep(500); }
          Console.WriteLine("\nYour cards:\n");
          player.ShowCards();
          if (scoreBoard)
          {
            ShowScoreBoard(true);
          }

          Console.WriteLine("");
          Console.Write("\n[D]raw | [S]tay | [T]oggle score board. ");

          switch (Console.ReadKey().Key)
          {
            case ConsoleKey.D:
              DrawACard(player);
              playing = false;
              break;

            case ConsoleKey.S:
              player.Score += player.CountScore();
              playing = false;
              player.Active = false;
              Console.WriteLine("");
              Console.WriteLine($"\nYour score this round is {player.CountScore()}");
              Console.WriteLine($"Your total score is {player.Score}");
              Console.Write("\nPress any key to continue. ");
              Console.ReadKey(true);
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
          TryClear();
          Console.WriteLine($"\nRound {roundCount}.\n");
          Console.WriteLine($"\n{player.Name}'s turn.\n");
          Console.WriteLine("\nYour cards:\n");
          player.ShowCards();
          if (scoreBoard)
          {
            ShowScoreBoard(true);
          }
          if (activePlayersCount > 1)
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
                Console.Write("\nPress any key to continue. ");
                Console.ReadKey(true);
                player.EmptyPlayerCards(deck);
                continue;
            }
          }
        }
      }
      bool activePlayers = false;
      activePlayersCount = 0;
      foreach (Player player in playersList)
      {
        if (player.Active == true)
        {
          activePlayers = true;
          activePlayersCount++;
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
    Console.WriteLine($"\nScores after round {roundCount}.");
    ShowScoreBoard(false);
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
// Method to create player.
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
// Method to show score board.
void ShowScoreBoard(bool inRound)
{
  Console.WriteLine("\n------------------------------");
  Console.WriteLine("\nSCORE BOARD\n");
  foreach (Player player in playersList)
  {
    if (inRound)
    { Console.WriteLine($"{player.Name}: {player.Score} | {(player.Active ? "playing" : "out")}"); }
    else { Console.WriteLine($"{player.Name}: {player.Score}"); }
  }
  Console.WriteLine("\n------------------------------");
}

// Method to select a player to freeze/flip 3
Player SelectedPlayer(Player player)
{
  List<Player> activePlayersList = new();
  foreach (Player activePlayer in playersList)
  {
    if (activePlayer.Active == true)
    { activePlayersList.Add(activePlayer); }
  }
  foreach (Player freezePlayer in activePlayersList)
  {
    if (freezePlayer == player)
    { Console.WriteLine($"[{activePlayersList.IndexOf(freezePlayer) + 1}] (myself) || my cards are worth: {freezePlayer.CountScore()} points."); }
    else
    { Console.WriteLine($"[{activePlayersList.IndexOf(freezePlayer) + 1}] {freezePlayer.Name} || their cards are worth: {freezePlayer.CountScore()} points."); }
  }
  Player? selectedPlayer = null;
  bool selectingPlayer = true;

  while (selectingPlayer)
  {
    Console.Write($"\nSelect index [1{(activePlayersList.Count > 1 ? "-" + activePlayersList.Count + "]" : "]")}: ");
    if (int.TryParse(Console.ReadLine(), out int selectedPlayerIndex) && selectedPlayerIndex > 0 && selectedPlayerIndex <= activePlayersList.Count)
    {
      selectedPlayer = activePlayersList[selectedPlayerIndex - 1];
      break;
    }
    else { continue; }
  }
  Debug.Assert(selectedPlayer != null);
  return selectedPlayer;
}
// Method to draw a card.
void DrawACard(Player player)
{
  Card? drawnCard = deck.CardDeck[RandomCard()];
  Console.WriteLine($"");
  Console.WriteLine($"\nDrawn card:");
  Console.Write($" ");
  Console.BackgroundColor = drawnCard.BackColor;
  Console.ForegroundColor = drawnCard.FrontColor;
  Console.WriteLine($"\n{drawnCard.CardInfo()}");
  Console.ResetColor();
  deck.CardDeck.Remove(drawnCard);
  Thread.Sleep(1000);
  switch (drawnCard.CardType)
  {
    case CardType.Modifier: player.PlayerCards.Add(drawnCard); return;
    case CardType.Double: player.PlayerCards.Add(drawnCard); return;
    case CardType.SecondChance: player.PlayerCards.Add(drawnCard); return;
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
          Console.Write("\nPress any key to continue. ");
          Console.ReadKey(true);
          player.PlayerCards.Add(drawnCard);
          player.EmptyPlayerCards(deck);
          player.Active = false;
          return;
        }
        else
        {
          deck.DiscardPile.Add(drawnCard);
          Console.WriteLine("\nThat was close! Luckily you had a Second Chance to save you!");
          Console.Write("\nPress any key to continue. ");
          Console.ReadKey(true);
          return;
        }
      }
      player.PlayerCards.Add(drawnCard);
      if (player.CheckFlip7())
      {
        player.Score += player.CountScore();
        player.Score += 15;
        Console.WriteLine("\n F L I P  7 !");
        Console.WriteLine($"\nYour score this round is {player.CountScore() + 15}");
        Console.WriteLine($"Your total score is {player.Score}");
        Console.Write("\nPress any key to continue. ");
        Console.ReadKey(true);
        player.EmptyPlayerCards(deck);
        player.Active = false;
      }
      return;

    case CardType.Freeze:
      deck.DiscardPile.Add(drawnCard);
      Console.WriteLine($"\nFreezing starts in");
      for (int i = 3; i > 0; --i)
      {
        switch (i)
        {
          case 3: Console.WriteLine($"     >>>{i}<<<"); break;
          case 2: Console.WriteLine($"      >>{i}<<"); break;
          case 1: Console.WriteLine($"       >{i}<"); break;
        }
        Thread.Sleep(1000);
      }

      TryClear();
      Console.WriteLine("\nF R E E Z E");
      ShowScoreBoard(true);

      Console.WriteLine("\nSelect player to freeze:\n");

      Player playerToFreeze = SelectedPlayer(player);

      playerToFreeze.Score += playerToFreeze.CountScore();
      Console.WriteLine("");
      Console.WriteLine($"\n{playerToFreeze.Name} is now frozen for this round!");
      Console.WriteLine($"\n{playerToFreeze.Name}'s score this round is {playerToFreeze.CountScore()}");
      Console.WriteLine($"{playerToFreeze.Name}'s total score is {playerToFreeze.Score}");
      playerToFreeze.EmptyPlayerCards(deck);
      playerToFreeze.Active = false;
      Console.Write("\nPress any key to continue. ");
      Console.ReadKey(true);
      return;

    case CardType.FlipThree:
      deck.DiscardPile.Add(drawnCard);
      ShowScoreBoard(true);
      Player playerToFlip3 = SelectedPlayer(player);
      bool flipping = true;
      int flipped = 3;
      while (flipping)
      {
        TryClear();
        Console.WriteLine("");
        Console.WriteLine($"\n{playerToFlip3.Name}'s have to flip {flipped} {(flipped == 3 ? "card" : "more card") + (flipped > 1 ? "s." : ".")}");
        Console.WriteLine($"\n{playerToFlip3.Name}'s cards:\n");
        playerToFlip3.ShowCards();
        if (playerToFlip3.Active && flipped > 0)
        {
          DrawACard(playerToFlip3); flipped--;
        }
        else
        {
          flipping = false;
          break;
        }
        if (flipped > 0)
        {
          Console.Write("\nPress any key to flip next card. ");
          Console.ReadKey(true);
        }
        else if (playerToFlip3.Active)
        {
          Console.Write("\nPress any key to continue. ");
          Console.ReadKey(true);
        }
      }
      return;
  }
}