using JobPortal.Data;
using JobPortal.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Models.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public EmployerRepository (ApplicationDbContext dbcontext) { 
            _dbContext = dbcontext;
        }
        public async Task<List<Employer>> GetEmpsAsync (int count) {
            return await _dbContext.Employers
                .Take(count)
                .ToListAsync();
        }

        public async Task<Employer?> GetByEmailAsync(string email)
        {
            return await _dbContext.Employers
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.User.Email == email);
        }


    }
}
