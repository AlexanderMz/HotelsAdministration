using System;
using System.Threading.Tasks;

namespace HotelsAdministration.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IHotelRepository Hotels { get; }
    IHotelReservationRepository HotelReservations { get; }
    ITravelerRepository Travelers { get; }
    Task CompleteAsync();
}