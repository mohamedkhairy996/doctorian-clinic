using clinic.Domain.models;
using clinic.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace clinic.Infrastructure.Implementations
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly applicationContext _dbContext;
        public ApplicationUserRepository(applicationContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _dbContext.ApplicationUsers.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllExceptAsync(string excludedUserId)
        {
            return await _dbContext.ApplicationUsers
                .Where(u => u.Id != excludedUserId)
                .ToListAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            _dbContext.ApplicationUsers.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            _dbContext.ApplicationUsers.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
