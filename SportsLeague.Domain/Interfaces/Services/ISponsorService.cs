using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllSponsorsAsync();
        Task<Sponsor?> GetSponsorByIdAsync(int id);
        Task<Sponsor> CreateSponsorAsync(Sponsor sponsor);
        Task UpdateSponsorAsync(int id, Sponsor sponsor);
        Task DeleteSponsorAsync(int id);

        Task LinkSponsorToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount);
        Task UnlinkSponsorFromTournamentAsync(int sponsorId, int tournamentId);
        Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId);
    }
}
