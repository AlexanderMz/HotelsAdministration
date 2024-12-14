using HotelsAdministration.Domain.Models;
namespace HotelsAdministration.Application.Interfaces;

public interface IEmailService
{
    Task SendReservationConfirmationAsync(Reservation reservation, Hotel hotel);
}