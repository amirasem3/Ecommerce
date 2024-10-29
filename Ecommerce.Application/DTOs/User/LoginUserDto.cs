namespace Ecommerce.Application.DTOs.User;

public class LoginUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public override string ToString()
    {
        return $"\n\tUsername : {Username}" +
               $"\n\tPassword : {Password}";
    }
}