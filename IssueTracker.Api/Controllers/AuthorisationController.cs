using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using IssueTracker.Dal.Models;

namespace IssueTracker.Controllers;

public class LoginDto
{
    public required string Login { get; set; }
    public required string Password { get; set; }

}

[ApiController]
[Route("api/[controller]")]
public class AuthorisationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private Dictionary<string, string> _authOptions;

    public AuthorisationController(
        UserManager<ApplicationUser> userManager
        , IConfiguration config
        )
    {
        _userManager = userManager;
        _config = config;
        _authOptions = _config.GetSection("AuthOptions").Get<Dictionary<string, string>>();
    }

    [Route("register")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Login);
        if (user != null)
        {
            return BadRequest(new { message = "User already exists"});
        }
        var appUser = new ApplicationUser();
        appUser.UserName = model.Login;
        
        var result = await _userManager.CreateAsync(appUser, model.Password);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(appUser, "User");
        else return BadRequest(new { message = "Something went wrong"});

        string userName = appUser.UserName;
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString())
        };

        var _token = GenerateAccessToken(claims);
        var newRefreshToken = GenerateRefreshToken();
        appUser.RefreshToken = newRefreshToken;
        appUser.RefreshTokenExpiry = DateTime.Now.AddDays(30).ToUniversalTime();
        await _userManager.UpdateAsync(appUser);

        var iUser = new
        {
            id = appUser.Id,
            username = userName,
            token = _token,
            refreshToken = newRefreshToken,
            role = "User"
        };

        return Ok(iUser);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Auth(LoginDto model)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(model.Login);
        if (user != null && user.PasswordHash != null) { 
            var pass = user.PasswordHash;
            var vali = _userManager.PasswordHasher.VerifyHashedPassword(user, pass, model.Password);
            if (vali != PasswordVerificationResult.Success)
            {
                return BadRequest(new { message = "Authorisation error: wrong password"});
            }
        }
        else
        {
            return BadRequest(new { message = "Authorisation error: user doesn't exist" });
        }

        string username = model.Login;
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        var _token = GenerateAccessToken(claims);
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.Now.AddDays(30).ToUniversalTime();
        await _userManager.UpdateAsync(user);

        bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        var iUser = new
        {
            id = user.Id,
            username = user.UserName,
            token = _token,
            refreshToken = newRefreshToken,
            role = isAdmin ? "Admin" : "User"
        };

        return Ok(iUser);
    }

    public class RefreshTokenRequest
    {
        public string accessToken { get; set; } = "";
        public string refreshToken { get; set; } = "";
    }

    [Route("refresh-token")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest) {

        var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.accessToken);
        var username = principal.Identity.Name; // Получаем имя пользователя из токена

        var user = await _userManager.FindByNameAsync(username);

        if (user == null
            || user.RefreshToken != refreshTokenRequest.refreshToken
            || user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            return BadRequest(new { message = "Invalid refresh token"});
        }

        var newAccessToken = GenerateAccessToken(principal.Claims);
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            token = newAccessToken,
        });
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        string key = _authOptions["KEY"];
        var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var jwt = new JwtSecurityToken(
            issuer: _authOptions["ISSUER"],
            audience: _authOptions["AUDIENCE"],
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));

        var _token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return _token;
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        string key = _authOptions["KEY"];
        var _token = token.Replace("Bearer ", "");
        var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = _key,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(_token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

}