public class Team
{
    public int TeamID { get; set; }
    public string TeamName { get; set; }
    public string Coach { get; set; }


    public List<Player> Players { get; set; } = new();
}

