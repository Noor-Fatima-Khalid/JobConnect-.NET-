namespace JobPortal.Models.Interfaces
{
    public interface IEmployerRepository
    {
        Task<List<Employer>> GetEmpsAsync(int count);
        Task<Employer> GetByEmailAsync(string email);            // this is for displaying the information of the currently logged in emp
    }
}
