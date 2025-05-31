namespace JobPortal.Models
{
    public class DashboardViewModel
    {
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<Resume> Resumes { get; set; } = new List<Resume>();
    }
}
