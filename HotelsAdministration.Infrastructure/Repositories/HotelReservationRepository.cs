using HotelsAdministration.Application.Configuration;
using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.DTOs;
using HotelsAdministration.Domain.Models.Enums;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelsAdministration.Infrastructure.Repositories
{
    public class HotelReservationRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings) : IHotelReservationRepository
    {
        private readonly IMongoCollection<Hotel> _hotelsCollection = database.GetCollection<Hotel>(settings.Value.HotelsCollectionName);
        private readonly IMongoCollection<Reservation> _reservationsCollection = database.GetCollection<Reservation>(settings.Value.ReservationsCollectionName);

        public async Task<IEnumerable<Hotel>> SearchHotelsAsync(HotelSearchDto searchDto)
        {
            var filter = Builders<Hotel>.Filter.And(
                Builders<Hotel>.Filter.Eq(h => h.City, searchDto.City),
                Builders<Hotel>.Filter.ElemMatch(h => h.Rooms,
                    r => r.Capacity >= searchDto.GuestsCount && r.IsAvailable)
            );

            return await _hotelsCollection.Find(filter).ToListAsync();
        }

        public async Task<Hotel> GetHotelByIdAsync(string id)
        {
            return await _hotelsCollection.Find(h => h.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            await _reservationsCollection.InsertOneAsync(reservation);
            return reservation;
        }

        public async Task<Reservation> GetReservationByIdAsync(string id)
        {
            return await _reservationsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateReservationStatusAsync(string id, ReservationStatus status)
        {
            var update = Builders<Reservation>.Update.Set(r => r.Status, status);
            var result = await _reservationsCollection.UpdateOneAsync(r => r.Id == id, update);
            return result.ModifiedCount > 0;
        }
    }
}