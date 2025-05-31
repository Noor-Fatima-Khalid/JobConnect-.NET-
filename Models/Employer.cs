using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Employer
    {
        public int Id { get; set; }
        // identity id is in string
        public string UserId { get; set; }         // FK
        public User User { get; set; }          // Nav prop

        [Required]
        public string Company { get; set; } = string.Empty;        // name. makes it non-nullable (did it while integrating ef core)
        public string? PhoneNo { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Industry { get; set; }        // as in finance, it, education
        public string? Description { get; set; }
        public int EmployeeCount { get; set; }
        public string? Website { get; set; }

        //public int JobId { get; set; }              //FK
        public List<Job> Jobs { get; set; } = new List<Job>();





        // THESE CONSTRUCTORS ARE TEMPORARY FOR HARDCODING IN APPLICANT SIDE!
        public Employer() { }
        public Employer(string company, string? industry)
        {
            Company = company;
            Industry = industry;
        }
        public Employer(string company, string email, string? phone, string location, string? industry, string? description, int employeeCount, string? website)
        {
            Company = company;
            User.Email = email;
            PhoneNo = phone;

            Location = location;
            Industry = industry;
            Description = description;
            EmployeeCount = employeeCount;
            Website = website;
        }
    }
}   