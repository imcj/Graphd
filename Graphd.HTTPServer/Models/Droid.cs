namespace Graphd.Tests.Models;

public class Droid
{
    public string Id { get; set; }

    public string Name { get; set; }

    public Factory? Factory { get; set; }

    public List<Tag> Tags { get; set; }
}
