public class Goal
{
    public int Id { get; set; } = Random.Shared.Next(1, int.MaxValue);
    public string Name { get; set; }
    public string Color { get; set; }
}