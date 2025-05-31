using System.Drawing;
using System.Security.AccessControl;
using JobPortal.Data;
using JobPortal.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace JobPortal.Models.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        public ApplicantRepository(UserManager<User> userManager, ApplicationDbContext context) { 
            _userManager = userManager;
            _dbContext = context;
        }
        public async Task<Applicant> GetById(string userId)
        {
            return await _dbContext.Applicants
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }


        public async Task UpdateProfileAsync(Applicant applicant, EditProfileViewModel model)
        {
            applicant.Name = model.Name;
            applicant.Location = model.Location;
            applicant.TaglineSkills = model.TaglineSkills;
            applicant.Skill = model.Skill;
            applicant.About = model.About;
            applicant.Education = model.Education;
            applicant.Certifications = model.Certifications;

            _dbContext.Applicants.Update(applicant);
            await _dbContext.SaveChangesAsync();
        }
    }
}
