namespace JobPortal.Models
{
    public class EmpProfileViewModel
    {
        public Employer CurrEmployer { get; set; }  // Company Details
        public List<Job> Jobs { get; set; } = new List<Job>();  // Job Listings - jo post ki hain
        public List<Employer> Employers { get; set; } = new List<Employer>(); // Store similar employers
    }
}
