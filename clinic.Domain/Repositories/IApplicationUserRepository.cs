
using clinic.Domain.models;

namespace clinic.Domain.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        // Add any additional methods specific to Category repository here
        // For example:
        // Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllExceptAsync(string excludedUserId);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(ApplicationUser user);

    }
}
