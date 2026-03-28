using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllAsync(bool showInactive);
        Task<Users?> GetByIdAsync(int id);
        Task<Users?> GetByUsernameAsync(string email);
        Task AddAsync(Users user);
        Task<bool> ValidateUsernme(string username);
        void Update(Users user);
        void Delete(Users user);
    }
}
