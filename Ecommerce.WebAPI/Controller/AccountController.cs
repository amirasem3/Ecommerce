using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Application.DTOs.User;
using Ecommerce.Application.Services;
using Ecommerce.Core.Log;
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
    private readonly UserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserService userService,ILogger<AccountController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("ApiSignin")]
    public async Task<IActionResult> Login([FromQuery] LoginUserDto loginUserDto)
    {
        LoggerHelper.LogWithDetails(_logger, args:[loginUserDto]);
        try
        {
            await _userService.AuthenticateUserAsync(loginUserDto.Username, loginUserDto.Password);

            const string issuer = "AmirHosseinIssuer";
            const string audience = "AmirHosseinAudience";
            var key = Encoding.ASCII.GetBytes(
                "This is a sample secret key - please don't use in production environment.");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUserDto.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, loginUserDto.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            tokenHandler.WriteToken(token);
            var tokenString = tokenHandler.WriteToken(token);

            //set Cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1),
                });

            
            var result  =new
            {
                token = tokenString,
            };
            LoggerHelper.LogWithDetails(_logger, "Generated Token",retrievedData:result);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Unauthorized(e);
        }
    }
}