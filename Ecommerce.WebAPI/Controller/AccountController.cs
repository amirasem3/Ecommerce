using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Interfaces;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceSolution.Controller;
[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserServices _userService;

    public AccountController(IUserServices userService)
    {
        _userService = userService;
    }
    [HttpPost("LoginApi")]
    public async Task<IActionResult> Login( [FromQuery]LoginUserDto loginModel)
    {
        var user = await _userService.AuthenticateUserAsync(loginModel.Username, loginModel.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        const string issuer = "AmirHosseinIssuer";
        const string audience = "AmirHosseinAudience";
        var key = Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment.");
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
      
        // var key = "SuperSecretKeyForTestingPurposes123!"u8.ToArray(); // Replace with your secret key
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // new Claim("Id",user.Id),
               new Claim(ClaimTypes.Name, loginModel.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var tokenString = tokenHandler.WriteToken(token);
        
        //set Cookie
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1),
            });

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };
        
        return Ok(new
        {
            token=tokenString,
            user
        });
        

    }
    
}