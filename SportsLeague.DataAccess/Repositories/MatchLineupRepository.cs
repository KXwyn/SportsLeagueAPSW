using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
    {
        public MatchLineupRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByMatchAndPlayerAsync(int matchId, int playerId)
        {
            return await _dbSet.AnyAsync(ml => ml.MatchId == matchId && ml.PlayerId == playerId);
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
        {
            return await _dbSet
                .Where(ml => ml.MatchId == matchId)
                .Include(ml => ml.Player) // Incluimos los datos del Jugador
                    .ThenInclude(p => p.Team) // Y dentro de Jugador, incluimos los datos de su Equipo
                .OrderByDescending(ml => ml.IsStarter) // Titulares primero
                .ThenBy(ml => ml.Player.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
        {
            return await _dbSet
                .Where(ml => ml.MatchId == matchId && ml.Player.TeamId == teamId)
                .Include(ml => ml.Player)
                .OrderByDescending(ml => ml.IsStarter) // Titulares primero
                .ThenBy(ml => ml.Player.LastName)
                .ToListAsync();
        }
    }
}