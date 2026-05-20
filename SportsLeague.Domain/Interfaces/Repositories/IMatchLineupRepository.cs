using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
    {

        // Obtiene la alineación completa para un partido, incluyendo los datos del jugador y su equipo.
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);

        // Obtiene la alineación de un equipo específico para un partido dado.
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);

        /// Verifica si un jugador ya está registrado en la alineación de un partido.
        Task<bool> ExistsByMatchAndPlayerAsync(int matchId, int playerId);
    }
}