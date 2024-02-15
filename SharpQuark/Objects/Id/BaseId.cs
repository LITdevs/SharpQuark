namespace SharpQuark.Objects.Id;

public class BaseId(string id)
{
    public string Id { get; } = id;

    public override string ToString()
    {
        return Id;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is BaseId other)
        {
            return Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public int CompareTo(BaseId other)
    {
        return string.Compare(Id, other.Id, StringComparison.Ordinal);
    }
}