using System;
using System.Runtime.CompilerServices;
using JobPortal.Data;
using JobPortal.Models.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Models.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public JobRepository(ApplicationDbContext _context) 
        { 
            _dbContext = _context;
        }
        public async Task<List<Job>> GetAllAsync()                  // for feed (in desc)
        {
            return await _dbContext.Job
                .Include(j => j.Employer)
                .OrderByDescending(j => j.PostedOn)
                .ToListAsync();
        }
        //public async Task<List<Job>> GetAllAsync()                  // this is for Applicant side JobSearch
        //{
        //    return await _dbContext.Job
        //        .Include(j => j.Employer)                       // for eager loading of employer into job table
        //        .ToListAsync();
        //}
        public async Task<List<Job>> GetJobsAsync(string userId)    // this is to show the jobs posted by each employer (jis ne jo post kia use wohi nazar aye ga)
        {
            // Get employer record for curr user
            var employer = await _dbContext.Employers
                .FirstOrDefaultAsync(e => e.UserId == userId);

            // validate
            if (employer == null)
                return new List<Job>(); 

            return await _dbContext.Job
                .Where(j => j.EmployerId == employer.Id)
                .ToListAsync();
        }
        public async Task<List<Job>> GetLatestJobsAsync(string userId, int count)       // for emp side
        {
            var employer = await _dbContext.Employers
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employer == null)
                return new List<Job>();

            return await _dbContext.Job
                .Where(j => j.EmployerId == employer.Id)  // filter jobs by employer
                .OrderByDescending(j => j.PostedOn)
                .Take(count)
                .ToListAsync();
        }


        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _dbContext.Job.FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<Job> AddJob(Job job)
        {
            await _dbContext.Job.AddAsync(job);
            await _dbContext.SaveChangesAsync();
            return job; 
        }

        public async Task DeleteJob(int id)
        {
            await _dbContext.Job.Where(j => j.Id == id).ExecuteDeleteAsync();
        }

        public async Task<Job?> UpdateJob(Job job)
        {
            var found = await _dbContext.Job.FindAsync(job.Id); 

            if (found != null)
            {
                found.Title = job.Title;
                found.Description = job.Description;
                found.EmpType = job.EmpType;
                found.Salary = job.Salary;
                found.Industry = job.Industry;
                found.Location = job.Location;
                found.Status = job.Status;
                found.PostedOn = job.PostedOn;

                await _dbContext.SaveChangesAsync();
            }
            return found;
        }
    }
}
