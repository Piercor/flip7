namespace App;

class Player
{
  public string Name;
  public int Score;
  public List<Card> PlayerCards = new();

  public Player(string name, int score)
  {
    Name = name;
    Score = score;
  }

  public string PlayerInfo()
  {
    return $"Player: {Name} || Score: {Score}";
  }
  public void CreatePlayer(string name)
  {
    Player newPlayer = new(name, 0);
  }
}