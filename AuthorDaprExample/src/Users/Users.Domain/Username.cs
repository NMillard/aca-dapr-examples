namespace Users.Domain;

public record Username
{
    private readonly string value;

    public required string Value
    {
        get => value;
        init
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            this.value = value.Length switch
            {
                < 3 => throw new ArgumentException("Too short"),
                > 50 => throw new ArgumentException("Too long"),
                _ => value
            };
        }
    }
}