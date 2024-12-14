using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using HotelsAdministration.Application.Configuration;
using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.Auth;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelsAdministration.Application.Services;


public class AuthService : IAuthService
{
    private readonly IMongoCollection<TravelAgent> _agents;
    private readonly IConfiguration _configuration;

    public AuthService(IOptions<MongoDbSettings> settings, IConfiguration configuration)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _agents = database.GetCollection<TravelAgent>("TravelAgents");
        _configuration = configuration;
    }

    public async Task<string> GenerateJwtToken(TravelAgent agent)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, agent.Id),
                new Claim(ClaimTypes.Email, agent.Email),
                new Claim(ClaimTypes.GivenName, agent.FirstName),
                new Claim(ClaimTypes.Surname, agent.LastName)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<TravelAgent> Authenticate(string email, string password)
    {
        var agent = await _agents.Find(x => x.Email == email).FirstOrDefaultAsync();

        if (agent == null || !VerifyPasswordHash(password, agent.PasswordHash))
            return null;

        return agent;
    }

    public async Task<TravelAgent> Register(RegisterRequest request)
    {
        if (await _agents.Find(x => x.Email == request.Email).AnyAsync())
            throw new Exception("Email already exists");

        var agent = new TravelAgent
        {
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _agents.InsertOneAsync(agent);
        return agent;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
}