namespace JobPortal.Models
{
    public enum ApplicationStatus
    {
        Pending,
        Accepted,
        Rejected,
    }
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public Job? Job { get; set; }
        public int ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }
        public DateTime AppliedOn { get; set; }
        
        public ApplicationStatus Status { get; set; }

        public int ResumeId { get; set; }           // resume foreign key
        public Resume? Resume { get; set; }         // removing it because an applicant could use same resume for multiple jobs
                                                    // this is a one resume to many jobs OR one resume to one applicant case 

    }
}
