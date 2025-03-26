public class CreatePlayerDTO
{
    public string Name { get; set; }
    public string Position { get; set; }
    public int TeamID { get; set; }  // Ensure it's an integer, not a string
}
