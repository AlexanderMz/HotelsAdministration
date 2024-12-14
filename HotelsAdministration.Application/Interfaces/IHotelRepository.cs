using System.Collections.Generic;
using System.Threading.Tasks;
using HotelsAdministration.Domain.Models;

namespace HotelsAdministration.Application.Interfaces;
public interface IHotelRepository
{
    Task<IEnumerable<Hotel>> GetAllAsync();
    Task<Hotel> GetByIdAsync(string id);
    Task<Hotel> CreateAsync(Hotel hotel);
    Task UpdateAsync(string id, Hotel hotel);
    Task<bool> ToggleHotelStatusAsync(string id, bool status);
    Task<bool> ToggleRoomStatusAsync(string hotelId, string roomNumber, bool status);
    Task<bool> UpdateRoomAsync(string hotelId, Room room);
}