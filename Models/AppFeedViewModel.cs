namespace JobPortal.Models
{
    public class AppFeedViewModel
    {
        public Applicant Applicant { get; set; }
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<Employer> Employers { get; set; } = new List<Employer>();
    }
}
