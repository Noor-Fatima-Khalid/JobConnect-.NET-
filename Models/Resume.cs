namespace JobPortal.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Location { get; set; }
        public string? Field { get; set; }
        //public string? Skill { get; set; }
        //public string? Experience { get; set; }
        public string? FilePath { get; set; }

        // each person has a resume, same one can be used for applying at various jobs so i added it to applicant.cs
        public int ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

    }
}
