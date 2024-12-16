using HotelsAdministration.Application.Configuration;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Domain.Models;


namespace HotelsAdministration.Infrastructure.Repositories;

public class HotelRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
    : IHotelRepository
{
    private readonly IMongoCollection<Hotel> _hotels = database.GetCollection<Hotel>(settings.Value.HotelsCollectionName);

    public async Task<IEnumerable<Hotel>> GetAllAsync()
    {
        return await _hotels.Find(_ => true).ToListAsync();
    }

    public async Task<Hotel> GetByIdAsync(string id)
    {
        return await _hotels.Find(h => h.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Hotel> CreateAsync(Hotel hotel)
    {
        await _hotels.InsertOneAsync(hotel);
        return hotel;
    }

    public async Task UpdateAsync(string id, Hotel hotel)
    {
        await _hotels.ReplaceOneAsync(h => h.Id == id, hotel);
    }

    public async Task<bool> ToggleHotelStatusAsync(string id, bool status)
    {
        var update = Builders<Hotel>.Update.Set(h => h.IsActive, status);
        var result = await _hotels.UpdateOneAsync(h => h.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> ToggleRoomStatusAsync(string hotelId, string roomNumber, bool status)
    {
        var update = Builders<Hotel>.Update.Set(
            h => h.Rooms[-1].IsActive, status);
        var result = await _hotels.UpdateOneAsync(
            h => h.Id == hotelId && h.Rooms.Any(r => r.RoomNumber == roomNumber),
            update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateRoomAsync(string hotelId, Room room)
    {
        var update = Builders<Hotel>.Update.Set(
            h => h.Rooms[-1], room);
        var result = await _hotels.UpdateOneAsync(
            h => h.Id == hotelId && h.Rooms.Any(r => r.RoomNumber == room.RoomNumber),
            update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> AddRoomAsync(string hotelId, Room room)
    {
        var filter = Builders<Hotel>.Filter.Eq(h => h.Id, hotelId);
        var update = Builders<Hotel>.Update.Push(h => h.Rooms, room);
        var result = await _hotels.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }
}