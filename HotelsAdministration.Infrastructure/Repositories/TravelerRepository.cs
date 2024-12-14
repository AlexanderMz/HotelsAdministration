using HotelsAdministration.Application.Configuration;
using HotelsAdministration.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotelsAdministration.Infrastructure.Repositories
{
    public class TravelerRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings) : ITravelerRepository
    {
        private readonly IMongoCollection<Traveler> _travelersCollection = database.GetCollection<Traveler>(settings.Value.TravelerCollectionName);

        public async Task<IEnumerable<Traveler>> GetAllTravelersAsync()
        {
            return await _travelersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Traveler> GetTravelerByIdAsync(string id)
        {
            return await _travelersCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Traveler> GetTravelerByDocumentAsync(string documentNumber)
        {
            return await _travelersCollection.Find(t => t.DocumentNumber == documentNumber).FirstOrDefaultAsync();
        }

        public async Task<Traveler> CreateTravelerAsync(Traveler traveler)
        {
            await _travelersCollection.InsertOneAsync(traveler);
            return traveler;
        }

        public async Task<bool> UpdateTravelerAsync(string id, Traveler traveler)
        {
            var result = await _travelersCollection.ReplaceOneAsync(t => t.Id == id, traveler);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTravelerAsync(string id)
        {
            var result = await _travelersCollection.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }
    }
}