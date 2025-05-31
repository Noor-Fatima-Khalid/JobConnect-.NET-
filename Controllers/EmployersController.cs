using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using JobPortal.Data;
using System;
using JobPortal.Hubs;
using JobPortal.Models;
using JobPortal.Models.Interfaces;
using JobPortal.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
    [Authorize(Policy = "EmployerOnly")]
    public class EmployersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJobRepository _jobRepository;
        private readonly IHubContext<NotificationHub> _hubObj;
        private readonly IResumeRepository _resumeRepository;
        private readonly IEmployerRepository _employerRepository;

        public EmployersController(ApplicationDbContext dbContext,
                           IResumeRepository resumeRepository, IJobRepository jobRepository, IHubContext<NotificationHub> hubObj, IEmployerRepository employerRepository)
        {
            _dbContext = dbContext;
            _resumeRepository = resumeRepository;
            _jobRepository = jobRepository;
            _hubObj = hubObj;  
            _employerRepository = employerRepository;
        }
        public async Task<ViewResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _jobRepository.GetLatestJobsAsync(userId, 4);
            var resumes = await _resumeRepository.GetTopResumesAsync(2);

            var model = new DashboardViewModel
            {
                Jobs = jobs,
                Resumes = resumes
            };

            return View(model);
        }


        public ViewResult Create()
        {
            return View();
        }


        public async Task<ViewResult> AllJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobs = await _jobRepository.GetJobsAsync(userId);
            return View(jobs);
        }

        
        [HttpGet]
        public ViewResult JobPost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JobPost(Job job)
        {
            // current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ModelState.AddModelError("", "User is not logged in.");
                return View();
            }

            // Find emp and validate
            var employer = await _dbContext.Employers.FirstOrDefaultAsync(e => e.UserId == userId);
            if (employer == null)
            {
                ModelState.AddModelError("", "Employer profile not found.");
                return View();
            }

            // Assignment: EmpId --> FK
            job.EmployerId = employer.Id;
            job.Status = "Open";
            job.PostedOn = DateTime.Now;

            await _jobRepository.AddJob(job);

            // Broadcasting notification
            // create notification
            var notification = new Notification
            {
                Message = $"New job posted: {job.Title}",
                SenderId = userId,
                CreatedAt = DateTime.Now
            };
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            // Step 2: Broadcast Notification via SignalR
            await _hubObj.Clients.All.SendAsync("ReceiveNotification", notification.Message);

            ViewBag.Message = "Job posted successfully!";
            return RedirectToAction("Dashboard", "Employers");
        }
            

        public async Task<ViewResult> Applicants()
        {
            var jobApplications = await _dbContext.JobApplications
            .Include(j => j.Applicant)
                .ThenInclude(a => a.User)  // If Applicant.User is needed
            .Include(j => j.Job)
            .ToListAsync();

            return View(jobApplications);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new EmpProfileViewModel();
            // 3 employers to show in similar interests card
            model.Employers = await _employerRepository.GetEmpsAsync(3);
            // show details of current company whose account is open
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                model.CurrEmployer = await _employerRepository.GetByEmailAsync(email);
            }
            // list of jobs
            model.Jobs = await _jobRepository.GetLatestJobsAsync(userId, 10);

            return View(model);
        }


        public async Task<IActionResult> Settings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var employer = await _dbContext.Employers
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            return View(employer);
        }


    }
}