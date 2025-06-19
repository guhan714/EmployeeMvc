namespace Employee.BusinessLogic.Results.Dtos;

public class LoginDto
{
    public LoginDto(Guid userId, string username, string password)
    {
        UserId = userId;
        Username = username;
        Password = password;
    }

    public Guid UserId { get; set; }
    public string Username { get; init; }
    public string Password { get; init; }
}