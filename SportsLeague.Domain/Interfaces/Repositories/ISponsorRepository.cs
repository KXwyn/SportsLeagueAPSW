using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        //verify if a sponsor with the given name already exists
        Task<bool> ExistsByNameAsync(string name);
    }
}
