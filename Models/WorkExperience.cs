namespace JobPortal.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }                    // Primary Key
        public int ApplicantId { get; set; }           // Foreign Key

        public string? Position { get; set; }
        public string? Company { get; set; }

        // these two are for filtering job seekers by dates
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }

        public string Duration { get; set; }
        public Applicant? Applicant { get; set; }      // navigation prop
    }
}
