namespace JobPortal.Models.Interfaces
{
    public interface IResumeRepository
    {
        Task<List<Resume>> GetTopResumesAsync(int count);
    }
}
