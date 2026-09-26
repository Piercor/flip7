using System.Data;
using System.Diagnostics;
using System.Drawing;
using App;

Deck deck = new();
List<Player> playersList = new();
List<ConsoleColor> colors = new([
    ConsoleColor.Blue,
    ConsoleColor.DarkYellow,
    ConsoleColor.DarkGreen,
    ConsoleColor.Magenta,
    ConsoleColor.White,
    ConsoleColor.DarkCyan,
    ConsoleColor.Red,
    ConsoleColor.DarkYellow,
    ConsoleColor.Gray,
    ConsoleColor.Green
]);


bool isRunning = true;

while (isRunning)
{
    GameUtility.TryClear();

    Console.WriteLine("\nFLIP 7\n");

    int pointsToReach = 200;

    bool inGame = false;

    string[] firstOptions = ["New game", "Quit"];
    switch (GameUtility.NavMenuKeys(firstOptions, false))
    {
        case 0:
            bool preGame = true;
            while (preGame)
            {
                GameUtility.TryClear();
                Console.WriteLine("\nHow many players (min. 3, max. 10)?");
                Console.Write("▶ ");

                if (int.TryParse(Console.ReadLine(), out int playersNumb) && playersNumb > 2 && playersNumb <= 10)
                {
                    GameUtility.TryClear();
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
                                playersList.Add(GameUtility.CreatePlayer(newPlayerName));
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
                    GameUtility.TryClear();
                    Console.WriteLine("\nPoints to reach? (200 by default)?");
                    Console.Write("▶ ");
                    if (int.TryParse(Console.ReadLine(), out int userPoints) && playersNumb > 2 && playersNumb <= 10)
                    {
                        pointsToReach = userPoints;
                    }

                    GameUtility.TryClear();
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
            GameUtility.TryClear();
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
                    GameUtility.TryClear();

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
                        GameUtility.ShowScoreBoard(true, playersList);
                    }
                    if (othersCards)
                    {
                        if (activePlayersCount > 1)
                        {
                            GameUtility.ShowOthersCards(player, playersList);
                            Console.WriteLine("------------------------------------------------\n");
                        }
                        else { othersCards = false; }
                    }

                    string[] mainOptions = ["Draw", "Stay", "Other players cards", "Score board", "Quit"];
                    switch (GameUtility.NavMenuKeys(mainOptions, true))
                    {
                        case 0:
                            GameUtility.DrawACard(player, deck, playersList);
                            playing = false;
                            break;

                        case 1:
                            Console.WriteLine("");
                            string[] confirmStay = ["Stay", "Keep playing"];
                            switch (GameUtility.NavMenuKeys(confirmStay, true))
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
                        case 4:
                            Console.WriteLine("");
                            string[] quitOptions = ["To main menu", "To desktop", "Cancel"];
                            switch (GameUtility.NavMenuKeys(quitOptions, true))
                            {
                                case 0:
                                    playing = false;
                                    round = false;
                                    inGame = false;
                                    playersList.Clear();
                                    break;
                                case 1:
                                    playing = false;
                                    round = false;
                                    inGame = false;
                                    isRunning = false;
                                    GameUtility.TryClear();
                                    break;
                                case 2: continue;
                            }
                            break;
                    }
                }
                if (round)
                {
                    if (player.Active)
                    {
                        GameUtility.TryClear();
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
                            GameUtility.ShowScoreBoard(true, playersList);
                        }
                        if (othersCards)
                        {
                            if (activePlayersCount > 1)
                            {
                                GameUtility.ShowOthersCards(player, playersList);
                                Console.WriteLine("------------------------------------------------\n");
                            }
                            else { othersCards = false; }
                        }
                        if (activePlayersCount > 1)
                        {
                            string[] stayOrContinue = ["Next player turn"];

                            switch (GameUtility.NavMenuKeys(stayOrContinue, true))
                            {
                                case 0: continue;
                            }
                        }
                    }
                }
                else { break; }
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

        bool winnerFound = playersList.Any(p => { p.Active = true; return p.Score >= pointsToReach; }) ? true : false;

        if (winnerFound)
        {
            playersList = playersList.OrderByDescending(p => p.Score).ToList();

            GameUtility.TryClear();
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
            playersList.Clear();
            break;
        }
        if (inGame)
        {
            GameUtility.TryClear();
            Console.WriteLine($"\nScores after round {roundCount}.");
            GameUtility.ShowScoreBoard(false, playersList);
            Console.Write("\nPress any key to start next round. ");
            Console.ReadKey(intercept: true);
            continue;
        }
        else { break; }
    }
}