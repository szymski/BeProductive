public class Goal
{
    public Goal()
    {
    }

    public Goal(string name)
    {
        Name = name;
    }

    public int Id { get; set; } = Random.Shared.Next(1, int.MaxValue);
    public string Name { get; set; }
}