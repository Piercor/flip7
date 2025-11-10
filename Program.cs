using System.Data;
using System.Diagnostics;
using System.Drawing;
using App;

Deck deck = new();
List<Player> playersList = new();
List<ConsoleColor> colors = new();
colors.Add(ConsoleColor.Blue);
colors.Add(ConsoleColor.DarkYellow);
colors.Add(ConsoleColor.DarkGreen);
colors.Add(ConsoleColor.Magenta);
colors.Add(ConsoleColor.White);
colors.Add(ConsoleColor.DarkCyan);
colors.Add(ConsoleColor.Red);
colors.Add(ConsoleColor.DarkYellow);
colors.Add(ConsoleColor.Gray);
colors.Add(ConsoleColor.Green);
int pointsToReach = 200;
bool isRunning = true;

while (isRunning)
{
  TryClear();

  Console.WriteLine("\nFLIP 7\n");
  bool inGame = false;
  string[] firstOptions = ["New game", "Quit"];
  switch (NavMenuKeys(firstOptions, false))
  {

    case 0:
      bool preGame = true;
      while (preGame)
      {
        TryClear();
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
              Console.BackgroundColor = colors[i];
              Console.ForegroundColor = ConsoleColor.Black;
              Console.Write($"\nPlayer {i + 1} name?");
              Console.ResetColor();
              Console.Write(" ");
              string? newPlayerName = Console.ReadLine();
              if (!string.IsNullOrWhiteSpace(newPlayerName))
              {
                playersList.Add(CreatePlayer(newPlayerName));
                playersList[i].Color = colors[i];
                creating = false;
              }
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
            Console.BackgroundColor = playersList[i].Color;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"{playersList[i].Name}");
            Console.ResetColor();
          }
          Console.WriteLine($"\nPoints to reach: {pointsToReach}.");
          Console.Write("\nPress ENTER when ready to play. ");
          Console.ReadLine();
          preGame = false;
        }
      }
      break;
    case 1:
      isRunning = false;
      break;
  }

  bool scoreBoard = true;
  bool othersCards = false;
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
        bool playing = false;
        if (player.Active == true)
        {
          playing = true;
        }
        while (playing)
        {
          TryClear();
          Console.WriteLine($"\nRound {roundCount}.\n");
          Console.BackgroundColor = player.Color;
          Console.ForegroundColor = ConsoleColor.Black;
          Console.WriteLine($"\n   {player.Name}'s turn.   \n");
          Console.ResetColor();
          Console.WriteLine($"Cards in deck: {deck.CardDeck.Count} || cards in discard pile: {deck.DiscardPile.Count}");
          Console.WriteLine($"\nYour cards: {(player.CountScore() > 0 ? $"(worth {player.CountScore()} pts.)" : "")}\n");
          player.ShowCards(true);
          if (scoreBoard)
          {
            ShowScoreBoard(true);
          }
          if (othersCards)
          {
            Console.WriteLine("\nOther players cards\n");
            foreach (Player showPlayer in playersList)
            {
              if (showPlayer != player && showPlayer.Active)
              {
                Console.WriteLine($"{showPlayer.Name} || their cards are worth: {showPlayer.CountScore()} points.");
                showPlayer.ShowCards(false);
              }
            }
          }
          string[] mainOptions = ["Draw", "Stay", "Other players cards", "Score board"];
          switch (NavMenuKeys(mainOptions, true))
          {
            case 0:
              DrawACard(player);
              playing = false;
              break;

            case 1:
              Console.WriteLine("");
              string[] confirmStay = ["Stay", "Keep playing"];
              switch (NavMenuKeys(confirmStay, true))
              {
                case 0:
                  player.Score += player.CountScore();
                  playing = false;
                  player.Active = false;
                  Console.WriteLine("");
                  Console.WriteLine($"\nYour score this round is {player.CountScore()}");
                  Console.WriteLine($"Your total score is {player.Score}");
                  Console.Write("\nPress any key to continue. ");
                  Console.ReadKey(intercept: true);
                  player.EmptyPlayerCards(deck);
                  break;
                case 1:
                  continue;
              }
              break;

            case 2:
              if (!othersCards) { othersCards = true; scoreBoard = false; }
              else { othersCards = false; }
              break;

            case 3:
              if (!scoreBoard) { scoreBoard = true; othersCards = false; }
              else { scoreBoard = false; }
              break;
          }

        }
        if (player.Active)
        {
          TryClear();
          Console.WriteLine($"\nRound {roundCount}.\n");
          Console.BackgroundColor = player.Color;
          Console.ForegroundColor = ConsoleColor.Black;
          Console.WriteLine($"\n   {player.Name}'s turn.   \n");
          Console.ResetColor();
          Console.WriteLine($"Cards in deck: {deck.CardDeck.Count} || cards in discard pile: {deck.DiscardPile.Count}");
          Console.WriteLine($"\nYour cards: {(player.CountScore() > 0 ? $"(worth {player.CountScore()} pts.)" : "")}\n");
          player.ShowCards(true);
          if (scoreBoard)
          {
            ShowScoreBoard(true);
          }
          if (othersCards)
          {
            Console.WriteLine("\nOther players cards\n");
            foreach (Player showPlayer in playersList)
            {
              if (showPlayer != player && showPlayer.Active)
              {
                Console.WriteLine($"{showPlayer.Name} || their cards are worth: {showPlayer.CountScore()} points.");
                showPlayer.ShowCards(false);
              }
            }
          }
          if (activePlayersCount > 1)
          {
            Console.WriteLine("");
            string[] stayOrContinue = ["Continue", "Stay"];

            switch (NavMenuKeys(stayOrContinue, true))
            {
              case 0: continue;
              case 1:
                Console.WriteLine("");
                string[] confirmStay = ["Confirm", "Keep playing"];
                switch (NavMenuKeys(confirmStay, true))
                {
                  case 0:
                    player.Score += player.CountScore();
                    playing = false;
                    player.Active = false;
                    Console.WriteLine("");
                    Console.WriteLine($"\nYour score this round is {player.CountScore()}");
                    Console.WriteLine($"Your total score is {player.Score}");
                    Console.Write("\nPress any key to continue. ");
                    Console.ReadKey(intercept: true);
                    player.EmptyPlayerCards(deck);
                    break;
                  case 1:
                    Console.WriteLine("");
                    Console.Write("\nYou still playing. Press ENTER to continue. ");
                    Console.ReadKey(intercept: true);
                    break;
                }
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
      break;
    }
    TryClear();
    Console.WriteLine($"\nScores after round {roundCount}.");
    ShowScoreBoard(false);
    Console.Write("\nPress any key to start next round. ");
    Console.ReadKey(intercept: true);
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
  Console.WriteLine("\nSCORE BOARD\n");
  foreach (Player player in playersList)
  {
    if (inRound)
    {
      Console.Write($"{player.Name}: {player.Score} | {(player.Active ? "playing" : "out")}");
    }
    else { Console.Write($"{player.Name}: {player.Score}"); }
    if ((playersList.IndexOf(player) + 1) % 2 != 0) { Console.Write(" |    | "); }
    if ((playersList.IndexOf(player) + 1) % 2 == 0) { Console.WriteLine(""); }

  }
  Console.WriteLine("\n------------------------------");
}

// Method to select a player to freeze/flip 3
Player SelectedPlayer(Player player, bool freeze)
{
  List<Player> activePlayersList = new();
  foreach (Player activePlayer in playersList)
  {
    if (activePlayer.Active == true)
    { activePlayersList.Add(activePlayer); }
  }
  string[] activePlayersArray = new string[activePlayersList.Count];
  Console.WriteLine("");
  foreach (Player chosePlayer in activePlayersList)
  {
    if (chosePlayer == player)
    {
      Console.BackgroundColor = chosePlayer.Color;
      Console.ForegroundColor = ConsoleColor.Black;
      Console.Write($" (myself) ");
      Console.ResetColor();
      Console.WriteLine($"|| my cards are worth: {chosePlayer.CountScore()} points. My score right now is {chosePlayer.Score} pts.");
      activePlayersArray[activePlayersList.IndexOf(chosePlayer)] = $"(myself)";
    }
    else
    {
      Console.BackgroundColor = chosePlayer.Color;
      Console.ForegroundColor = ConsoleColor.Black;
      Console.Write($" {chosePlayer.Name} ");
      Console.ResetColor();
      Console.WriteLine($"|| their cards are worth: {chosePlayer.CountScore()} points. Their score right now is {chosePlayer.Score} pts.");
      activePlayersArray[activePlayersList.IndexOf(chosePlayer)] = $"{chosePlayer.Name}";
    }

    chosePlayer.ShowCards(false);
  }
  Console.WriteLine($"\nSelect player to {(freeze ? "freeze" : "flip 3")} :\n");
  Player? selectedPlayer = activePlayersList[NavMenuKeys(activePlayersArray, false)];
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
  deck.DeckReshuffle();
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
          Console.ReadKey(intercept: true);
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
          Console.ReadKey(intercept: true);
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
        Console.ReadKey(intercept: true);
        player.EmptyPlayerCards(deck);
        player.Active = false;
      }
      return;

    case CardType.Freeze:
      deck.DiscardPile.Add(drawnCard);
      Thread.Sleep(1000);
      TryClear();
      Console.WriteLine("\nF R E E Z E");

      Console.WriteLine("\nSelect player to freeze:\n");

      Player playerToFreeze = SelectedPlayer(player, true);
      TryClear();
      playerToFreeze.Score += playerToFreeze.CountScore();
      Console.WriteLine("\n");
      Console.BackgroundColor = playerToFreeze.Color;
      Console.ForegroundColor = ConsoleColor.Black;
      Console.Write($"\n{playerToFreeze.Name}");
      Console.ResetColor();
      Console.WriteLine(" is now frozen for this round!");
      Console.WriteLine($"\n{playerToFreeze.Name}'s score this round is {playerToFreeze.CountScore()}");
      Console.WriteLine($"{playerToFreeze.Name}'s total score is {playerToFreeze.Score}");
      playerToFreeze.EmptyPlayerCards(deck);
      playerToFreeze.Active = false;
      Console.Write("\nPress any key to continue. ");
      Console.ReadKey(intercept: true);
      return;

    case CardType.FlipThree:
      deck.DiscardPile.Add(drawnCard);
      Thread.Sleep(1000);
      TryClear();
      Console.WriteLine("\nF L I P  3");
      Player playerToFlip3 = SelectedPlayer(player, false);
      bool flipping = true;
      int flipped = 3;
      while (flipping)
      {
        TryClear();
        Console.WriteLine("");
        Console.BackgroundColor = playerToFlip3.Color;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write($"\n{playerToFlip3.Name}");
        Console.ResetColor();
        Console.WriteLine($" have to flip {flipped} {(flipped == 3 ? "card" : "more card") + (flipped > 1 ? "s." : ".")}");
        Console.WriteLine($"\n{playerToFlip3.Name}'s cards:\n");
        playerToFlip3.ShowCards(false);
        if (playerToFlip3.Active && flipped > 0)
        {
          DrawACard(playerToFlip3); flipped--;
        }
        else
        {
          flipping = false;
          break;
        }
        if (playerToFlip3.Active && flipped > 0)
        {
          Console.Write("\nPress any key to flip next card. ");
          Console.ReadKey(intercept: true);
        }
        else if (playerToFlip3.Active)
        {
          Console.Write("\nPress any key to continue. ");
          Console.ReadKey(intercept: true);
        }
      }
      return;
  }
}
void NavMenu(int selectedIndex, string[] menuOptions, bool horizontal)
{
  int cursorPosition = Console.CursorTop;
  Console.SetCursorPosition(0, cursorPosition);
  for (int i = 0; i < menuOptions.Length; ++i)
  {
    if (i == selectedIndex)
    {
      Console.BackgroundColor = ConsoleColor.DarkMagenta;
      if (horizontal)
      { Console.Write($"  {menuOptions[i]} "); }
      else { Console.WriteLine($"  {menuOptions[i]} "); }
      Console.ResetColor();
    }
    else
    {
      if (horizontal)
      { Console.Write($" {menuOptions[i]}  "); }
      else { Console.WriteLine($" {menuOptions[i]}  "); }
    }
  }
  Console.SetCursorPosition(0, cursorPosition);
}
int NavMenuKeys(string[] menuOptions, bool horizontal)
{
  ConsoleKey? upLeft = ConsoleKey.UpArrow;
  ConsoleKey? downRight = ConsoleKey.DownArrow;
  if (horizontal)
  {
    upLeft = ConsoleKey.LeftArrow;
    downRight = ConsoleKey.RightArrow;
  }

  bool inMenu = true;
  int selectedIndex = 0;
  int selectedOption = 0;
  while (inMenu)
  {
    NavMenu(selectedIndex, menuOptions, horizontal);
    switch (Console.ReadKey(intercept: true).Key)
    {
      case var k when k == upLeft:
        selectedIndex--;
        if (selectedIndex < 0)
        { selectedIndex = menuOptions.Length - 1; }
        break;
      case var k when k == downRight:
        selectedIndex++;
        if (selectedIndex >= menuOptions.Length)
        { selectedIndex = 0; }
        break;
      case ConsoleKey.Enter:
        selectedOption = selectedIndex;
        inMenu = false;
        break;
    }
  }
  return selectedOption;
}