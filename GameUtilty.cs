namespace App;

public static class GameUtility
{
    // Method to create a menu (only used by NavMenuKeys) 
    public static void NavMenu(int selectedIndex, string[] menuOptions, bool horizontal)
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

    // Method to use arrows to select menu options 
    public static int NavMenuKeys(string[] menuOptions, bool horizontal)
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

    // Method to clear console.
    public static void TryClear()
    {
        try { Console.Clear(); } catch { }
    }

    // Method to create player.
    public static Player CreatePlayer(string name)
    {
        return new(name, 0);
    }

    // Method to draw a random card.
    public static int RandomCard(Deck deck)
    {
        Random rnd = new();

        return rnd.Next(deck.CardDeck.Count);
    }

    // Method to draw a card.
    public static void DrawACard(Player player, Deck deck, List<Player> playersList)
    {
        Card? drawnCard = deck.CardDeck[RandomCard(deck)];

        Console.WriteLine($"");
        Console.WriteLine($"\nDrawn card:");
        Console.Write($" ");
        Console.BackgroundColor = drawnCard.BackColor;
        Console.ForegroundColor = drawnCard.FrontColor;
        Console.WriteLine($"\n{drawnCard.CardInfo()}");
        Console.ResetColor();

        deck.CardDeck.Remove(drawnCard);
        deck.DeckReshuffle();
        Thread.Sleep(500);

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

                Player playerToFreeze = SelectedPlayer(player, true, playersList);

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

                Player playerToFlip3 = SelectedPlayer(player, false, playersList);
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
                        DrawACard(playerToFlip3, deck, playersList); flipped--;
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

    // Method to show score board.
    public static void ShowScoreBoard(bool inRound, List<Player> playersList)
    {
        int longestName = playersList.OrderByDescending(p => p.Name.Length).ToList()[0].Name.Length;

        Console.WriteLine("\nSCORE BOARD\n");

        foreach (Player player in playersList)
        {
            if (playersList.IndexOf(player) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
            Console.ForegroundColor = ConsoleColor.White;

            if (inRound)
            {
                Console.Write($"{player.Name}  ");
                for (int i = 0; i < (longestName - player.Name.Length); ++i) { Console.Write(" "); }
                Console.Write($"|{(player.Score < 100 ? " " : "")}{(player.Score < 10 ? " " : "")} {player.Score} pts.  ||  {(player.Active ? "playing" : "    out")}");
                Console.WriteLine();
            }
            else
            {
                Console.Write($"{player.Name}  ");
                for (int i = 0; i < (longestName - player.Name.Length); ++i) { Console.Write(" "); }
                Console.Write($"|{(player.Score < 100 ? " " : "")}{(player.Score < 10 ? " " : "")} {player.Score}");
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        if (longestName < 24) { Console.Write("\n------------------------------------------------"); }
        else
        {
            Console.Write("\n-----------------------");
            for (int i = 0; i < longestName; ++i) { Console.Write("-"); }
        }
        Console.WriteLine("\n");
    }

    // Method to show other players cards
    public static void ShowOthersCards(Player player, List<Player> playersList)
    {
        Console.WriteLine("\nOther players cards\n");

        foreach (Player showPlayer in playersList)
        {
            if (showPlayer != player && showPlayer.Active)
            {
                Console.WriteLine($"{showPlayer.Name} {(showPlayer.PlayerCards.Count > 0 ? $"|| their cards are worth: {showPlayer.CountScore()} points." : "has no cards yet.")}");
                if (showPlayer.PlayerCards.Count > 0) { showPlayer.ShowCards(false); }
            }
        }
    }

    // Method to select a player to freeze/flip 3
    public static Player SelectedPlayer(Player player, bool freeze, List<Player> playersList)
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
}