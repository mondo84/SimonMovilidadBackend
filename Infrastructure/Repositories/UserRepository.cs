using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<List<Users>> GetAllAsync(bool showInactive)
        {
            var query = _context.Users
                    .Where(w => w.Role != null)
                    .Include(join => join.Role)
                    .AsNoTracking();

            if (!showInactive)
                query = query.Where(u => u.Active);

            return await query.ToListAsync();
        }

        public async Task<Users?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public void Delete(Users user)
        {
            _context.Users.Remove(user);
        }

        public void Update(Users user)
        {
            _context.Users.Update(user);
        }

        public async Task<Users?> GetByUsernameAsync(string email)
        {
            var resp = await _context.Users
                .Include(join => join.Role).AsNoTracking()
                .FirstOrDefaultAsync(user => user.UserName == email);
            return resp;
        }

        public async Task<bool> ValidateUsernme(string username)
        {
            return await _context.Users.AnyAsync(w => w.UserName == username);
        }
    }
}
