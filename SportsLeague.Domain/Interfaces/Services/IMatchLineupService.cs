using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
        Task<MatchLineup> AddPlayerToLineupAsync(int matchId, MatchLineup lineup);
        Task<IEnumerable<MatchLineup>> GetLineupAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetTeamLineupAsync(int matchId, int teamId);
        Task DeletePlayerFromLineupAsync(int matchId, int lineupId);
    }
}