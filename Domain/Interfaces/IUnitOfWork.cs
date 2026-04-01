using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ISensorRepository Sensors { get; }
        IAlertRepository Alerts { get; }
        Task<int> SaveChangesAsync();
    }
}
