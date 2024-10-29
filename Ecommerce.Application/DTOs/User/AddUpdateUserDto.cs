namespace Ecommerce.Application.DTOs.User;

public class AddUpdateUserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public override string ToString()
    {
        return $"\n \tFirstName: {FirstName}\n" +
               $"\tLastName: {LastName}\n" +
               $"\tUsername: {Username}\n" +
               $"\tEmail: {Email}\n" +
               $"\tPhoneNumber: {PhoneNumber}\n" +
               $"\tRoleName: {RoleName}";
    }
}