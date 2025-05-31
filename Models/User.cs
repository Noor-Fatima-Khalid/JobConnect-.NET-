using Microsoft.AspNetCore.Identity;

namespace JobPortal.Models
{
    public class User : IdentityUser
    {
        public enum Role
        {
            JobSeeker = 0,
            Employer = 1
        }

        public Role IsRole { get; set; }
        public Applicant? Applicant { get; set; }
        public Employer? Employer { get; set; }
    }

}
