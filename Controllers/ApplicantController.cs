using System.Reflection;
using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Models.Interfaces;
using JobPortal.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
    [Authorize(Policy = "JobSeekerOnly")]
    public class ApplicantController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ApplicationDbContext _dbContext;
        IJobRepository _jobRepository;
        IJobApplicationsRepository _applicationsRepository;
        IApplicantRepository _applicantRepository;
        IEmployerRepository _employerRepository;

        public ApplicantController(ApplicationDbContext context, UserManager<User> userManager, IJobRepository jobRepository, 
            IApplicantRepository applicantRepository, IEmployerRepository employerRepository, IWebHostEnvironment webHostEnvironment,
            IJobApplicationsRepository applicationsRepository) { 
            
            _dbContext = context;
            _userManager = userManager;
            _jobRepository = jobRepository;
            _applicantRepository = applicantRepository;
            _employerRepository = employerRepository;
            _webHostEnvironment = webHostEnvironment;
            _applicationsRepository = applicationsRepository;
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var applicant = await _dbContext.Applicants
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (applicant == null)
            {
                return NotFound("Applicant profile not found.");
            }

            await _applicantRepository.UpdateProfileAsync(applicant, model);

            return RedirectToAction("Feed", "Applicant");
        }


        public async Task<IActionResult> Feed()
        {
            var userId = _userManager.GetUserId(User);
            var applicant = await _dbContext.Applicants.FirstOrDefaultAsync(a => a.UserId == userId);

            var model = new AppFeedViewModel
            {
                Jobs = await _jobRepository.GetAllAsync(),
                Employers = await _employerRepository.GetEmpsAsync(3),
                Applicant = applicant
            };
            ViewBag.Applicant = applicant;
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ViewEmployer(int id)
        {
            // Get employer including related User to access UserId
            var employer = await _dbContext.Employers
                .Include(e => e.User)   // include user to avoid null reference
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employer == null)
                return NotFound();

            var jobs = await _jobRepository.GetJobsAsync(employer.UserId);

            var model = new EmpProfileViewModel
            {
                CurrEmployer = employer,
                Jobs = jobs
            };

            return View(model);
        }


        //public IActionResult SavedJobs() 
        //{
            //return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> JobSearchAsync(List<string> EmploymentTypes, List<string> Industries, List<string> Companies)
        //{
        //    if (_jobRepository == null)
        //    {
        //        throw new Exception("JobRepository is null. Check DI configuration.");
        //    }
        //    var jobs = await _jobRepository.GetAllAsync();
        //    if (EmploymentTypes != null && EmploymentTypes.Any())
        //        jobs = jobs.Where(j => EmploymentTypes.Contains(j.EmpType)).ToList();

        //    if (Industries != null && Industries.Any())
        //        jobs = jobs.Where(j => Industries.Contains(j.Industry)).ToList();

        //    if (Companies != null && Companies.Any())
        //        jobs = jobs.Where(j => Companies.Contains(j.Employer?.Company)).ToList();

        //    return PartialView("_JobListPartial", jobs);
        //}
        
        public async Task<IActionResult> MyApplications()
        {
            var userId = _userManager.GetUserId(User);
            var applicant = await _dbContext.Applicants.FirstOrDefaultAsync(a => a.UserId == userId);

            if (applicant == null)
            {
                return NotFound();
            }

            var model = await _applicationsRepository.GetbyAppIdAsync(applicant.Id);

            return View(model); 
        }

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            // error handling
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); 
            }

            //curr applicant
            var applicant = await _applicantRepository.GetById(userId);

            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        [AllowAnonymous]
        public async Task<IActionResult> NotificationsAsync()
        {
            var model = await _employerRepository.GetEmpsAsync(3);
            
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Apply(int jobId)
        {
            var model = new ApplyViewModel { JobId = jobId };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ResumeFile == null || model.ResumeFile.Length == 0)
            {
                ModelState.AddModelError("", "Please upload a resume.");
                return View(model);
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "resumes");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ResumeFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ResumeFile.CopyToAsync(stream);
            }

            var userId = _userManager.GetUserId(User);
            var applicant = await _dbContext.Applicants
                 .Include(a => a.User)
                 .FirstOrDefaultAsync(a => a.UserId == userId);

            Console.WriteLine(applicant.User.UserName);

            if (applicant == null || applicant.User == null)
            {
                ModelState.AddModelError("", "Applicant profile not found.");
                return View(model);
            }

            var resume = new Resume
            {
                Name = string.IsNullOrWhiteSpace(model.Resume?.Name) ? applicant.Name : model.Resume.Name,
                Email = string.IsNullOrWhiteSpace(model.Resume?.Email) ? applicant.User.Email : model.Resume.Email,
                Location = string.IsNullOrWhiteSpace(model.Resume?.Location) ? applicant.Location : model.Resume.Location,
                Field = model.Resume?.Field ?? "",
                FilePath = "/resumes/" + uniqueFileName,
                ApplicantId = applicant.Id
            };


            try
            {
                _dbContext.Resumes.Add(resume);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Resume saved with ID: {resume.Id}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Resume save failed: " + ex.Message);
                return View(model);
            }

            var jobApp = new JobApplication
            {
                JobId = model.JobId,
                ApplicantId = applicant.Id,
                ResumeId = resume.Id,
                AppliedOn = DateTime.Now,
                Status = ApplicationStatus.Pending
            };

            try
            {
                _dbContext.JobApplications.Add(jobApp);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Application saved for job {jobApp.JobId} with resume {jobApp.ResumeId}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Application save failed: " + ex.Message);
                return View(model);
            }

            ViewBag.Applicant = applicant;
            return RedirectToAction("MyApplications", "Applicant"); 
        }


    }
}
