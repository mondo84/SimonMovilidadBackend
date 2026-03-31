using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; }
        public ISensorRepository Sensors { get; }

        public IAlertRepository Alerts { get; }

        public UnitOfWork(AppDbContext context, IUserRepository userRepository, ISensorRepository sensors, IAlertRepository alerts)
        {
            _context = context;
            Users = userRepository;
            Sensors = sensors;
            Alerts = alerts;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
