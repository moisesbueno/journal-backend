namespace Journal.Domain.Entities;

public partial class User : Entity
{
    public string Email { get; protected set; }

    public string Password { get; set; }

    public User()
    {

    }

    public User(string email, string passWord)
    {
        Email = email;
        Password = passWord;
    }
}