using JobPortal.Data;
using JobPortal.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Models.Repositories
{
    public class JobApplicationsRepository : IJobApplicationsRepository
    {
        private readonly ApplicationDbContext _context;
        public JobApplicationsRepository (ApplicationDbContext context) { 
            _context = context;
        }
        // to get job application in order to populate my applications page
        public async Task<List<JobApplication>> GetbyAppIdAsync(int appId) {
            return await _context.JobApplications
                .Where(j => j.ApplicantId == appId)
                .Include(j => j.Job)
                .Include(j => j.Resume)
                .ToListAsync();
        }
    }
}
