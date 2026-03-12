# Flip 7 🃏

A command-line implementation of **Flip 7**, the press-your-luck card game, built with C# and .NET.

## About the Game

Flip 7 is a fast-paced card game created by Eric Olsen (all rights reserved), where players race to reach **200 points** across multiple rounds. On each turn, you choose to **Hit** (draw another card) or **Stay** (bank your current points). The catch: if you draw a duplicate number, you **bust** and score zero for the round!

The deck is a pyramid-style set of 94 cards — there is one `1`, two `2`s, three `3`s, and so on up to twelve `12`s — plus special Action and Modifier cards that shake up the game.

### Key Rules

- **Hit or Stay** — Draw another card or lock in your points.
- **Bust** — Drawing a duplicate number card means you score 0 for the round.
- **Flip 7 Bonus** — Collect 7 unique number cards in a single round to instantly end it and earn **+15 bonus points**.
- **Action Cards**
  - **Freeze** — Forces a chosen player to bank their points and exit the round.
  - **Flip Three** — Forces a chosen player to immediately draw 3 cards.
  - **Second Chance** — Saves you from one bust; both the duplicate and this card are discarded.
- **Modifier Cards** — `+2` through `+10` add flat bonus points; `X2` doubles your number card total before modifiers are applied.

The first player to reach 200 points (without busting that round) wins!

## Project Structure

```
flip7/
├── Program.cs      # Entry point and main game loop
├── Card.cs         # Card model (value, type)
├── CardType.cs     # Enum defining card types (Number, Action, Modifier)
├── Deck.cs         # Deck creation, shuffling, and dealing logic
├── Player.cs       # Player state (hand, score, bust status)
├── flip7.csproj    # .NET project file
└── flip7.sln       # Visual Studio solution file
```

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or later

## Getting Started

**Clone the repository:**

```bash
git clone https://github.com/Piercor/flip7.git
cd flip7
```

**Build the project:**

```bash
dotnet build
```

**Run the game:**

```bash
dotnet run
```

## How to Play (in the app)

1. Launch the game and enter the number of players and their names.
2. Select the score goal (200 by default).
3. Each round, you'll be asked **Hit** or **Stay**.
4. Keep drawing cards to build your score — but don't bust!
5. The round ends when all players have either stayed or busted (or someone hits 7 unique cards).
6. Scores accumulate across rounds until a player reaches **200 points**.

## License

This project is open source. Feel free to fork and extend it!
