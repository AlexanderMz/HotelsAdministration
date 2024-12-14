using System.Threading.Tasks;

namespace HotelsAdministration.Application.Interfaces;

public interface IUnitOfWork
{
    IHotelRepository Hotels { get; }
    Task CompleteAsync();
}