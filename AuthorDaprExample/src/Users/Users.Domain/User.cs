namespace Users.Domain;

public class User
{
    public Guid Id { get; set; }
    public Username Username { get; set; }

    public UserInfo UserInfo => new(Username.Value);
}