namespace SharpQuark.Objects.Id;

public class BaseId(string id)
{
    public string Id { get; } = id;

    public override string ToString()
    {
        return Id;
    }
}