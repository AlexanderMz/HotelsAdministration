using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.Auth;

namespace HotelsAdministration.Application.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtToken(TravelAgent agent);
    Task<TravelAgent> Authenticate(string email, string password);
    Task<TravelAgent> Register(RegisterRequest request);
}