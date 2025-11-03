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
}