
using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.DTOs;
using HotelsAdministration.Domain.Models.Enums;

public interface IHotelReservationRepository
{
    Task<IEnumerable<Hotel>> SearchHotelsAsync(HotelSearchDto searchDto);
    Task<Hotel> GetHotelByIdAsync(string id);
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<Reservation> GetReservationByIdAsync(string id);
}