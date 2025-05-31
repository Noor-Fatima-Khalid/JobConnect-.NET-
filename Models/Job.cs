using JobPortal.Models;

public class Job
{
    public int Id { get; set; }

    public int EmployerId { get; set; }         // FK to Employer
    public Employer? Employer { get; set; }     // Nav property

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Salary { get; set; }
    public string? Industry { get; set; }
    public string? EmpType { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public DateTime PostedOn { get; set; }

    public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public Job()
    {
        PostedOn = DateTime.Now;
    }
}
