using HotelsAdministration.Application.Interfaces;

namespace HotelsAdministration.Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public IHotelRepository Hotels { get; }

    public UnitOfWork(IHotelRepository hotelRepository)
    {
        Hotels = hotelRepository;
    }

    public async Task CompleteAsync()
    {
        // En MongoDB las operaciones son atómicas, pero aquí podrías
        // agregar lógica adicional si es necesario
        await Task.CompletedTask;
    }
}
