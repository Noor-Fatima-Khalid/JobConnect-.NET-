using System.Runtime.CompilerServices;
using JobPortal.Data;
using JobPortal.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Models.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ResumeRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
        }
        public async Task<List<Resume>> GetTopResumesAsync(int count)
        {
            return await _dbContext.Resumes
                .Take(count)
                .ToListAsync();
        }
    }
}
