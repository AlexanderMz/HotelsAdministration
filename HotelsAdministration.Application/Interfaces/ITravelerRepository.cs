
using HotelsAdministration.Domain.Models;

public interface ITravelerRepository
{
    Task<IEnumerable<Traveler>> GetAllTravelersAsync();
    Task<Traveler> GetTravelerByIdAsync(string id);
    Task<Traveler> GetTravelerByDocumentAsync(string documentNumber);
    Task<Traveler> CreateTravelerAsync(Traveler traveler);
    Task<bool> UpdateTravelerAsync(string id, Traveler traveler);
    Task<bool> DeleteTravelerAsync(string id);
}