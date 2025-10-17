using FastEndpoints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PotatoProject2.Data;
using PotatoProject2.Models;

namespace PotatoProject2.Endpoints;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginEndpoint : Endpoint<LoginRequest>
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public LoginEndpoint(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public override void Configure()
    {
        Post("/api/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // Έλεγχος χρήστη
        var user = _context.Users.SingleOrDefault(u => u.Username == req.Username && u.Password == req.Password);

        if (user == null)
        {
            HttpContext.Response.StatusCode = 401;
            await HttpContext.Response.WriteAsJsonAsync(new { Message = "Invalid username or password" }, ct);
            return;
        }

        // Δημιουργία JWT
        var jwtKey = _config["Jwt:Key"]!;
        var jwtIssuer = _config["Jwt:Issuer"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: null,
            claims: new[] { new Claim(ClaimTypes.Name, user.Username) },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        // Επιστροφή token
        HttpContext.Response.StatusCode = 200;
        await HttpContext.Response.WriteAsJsonAsync(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        }, ct);
    }
}
