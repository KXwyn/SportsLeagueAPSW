using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _lineupRepository;
        private readonly IMatchRepository _matchRepository; // Necesitamos este repositorio para validar el partido manualmente
        private readonly MatchValidationHelper _validationHelper;

        public MatchLineupService(
            IMatchLineupRepository lineupRepository,
            IMatchRepository matchRepository,
            MatchValidationHelper validationHelper)
        {
            _lineupRepository = lineupRepository;
            _matchRepository = matchRepository;
            _validationHelper = validationHelper;
        }

        public async Task<MatchLineup> AddPlayerToLineupAsync(int matchId, MatchLineup lineup)
        {
            // V1: Validamos que el partido exista manualmente (para evitar la validación de estado InProgress del helper)
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

            // V6: Validamos que el partido esté en estado Scheduled
            if (match.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException("Solo se pueden registrar alineaciones en partidos Scheduled");

            // V2 & V3: Validar que el jugador exista y pertenezca al equipo del partido.
            // Aquí SÍ reusamos el helper de manera excelente. Guardamos el resultado en una variable 'player'.
            var player = await _validationHelper.ValidatePlayerInMatchAsync(lineup.PlayerId, match);

            // V4: Validar que no esté ya registrado
            if (await _lineupRepository.ExistsByMatchAndPlayerAsync(matchId, lineup.PlayerId))
                throw new InvalidOperationException("El jugador ya está registrado en la alineación de este partido");

            // V5: Máximo 11 titulares por equipo
            if (lineup.IsStarter)
            {
                // Optimización: Usamos la variable 'player' que ya consultamos arriba, evitando ir a la BD otra vez.
                var currentLineup = await _lineupRepository.GetByMatchAndTeamAsync(matchId, player.TeamId);
                if (currentLineup.Count(l => l.IsStarter) >= 11)
                    throw new InvalidOperationException("El equipo ya tiene 11 titulares registrados en este partido");
            }

            lineup.MatchId = matchId;
            var created = await _lineupRepository.CreateAsync(lineup);

            // Re-consultamos para que el retorno incluya las propiedades de navegación que AutoMapper necesita
            var fullLineup = await _lineupRepository.GetByMatchAsync(matchId);
            return fullLineup.First(ml => ml.Id == created.Id);
        }

        public async Task<IEnumerable<MatchLineup>> GetLineupAsync(int matchId)
        {
            var matchExists = await _matchRepository.ExistsAsync(matchId);
            if (!matchExists)
                throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

            return await _lineupRepository.GetByMatchAsync(matchId);
        }

        public async Task<IEnumerable<MatchLineup>> GetTeamLineupAsync(int matchId, int teamId)
        {
            var matchExists = await _matchRepository.ExistsAsync(matchId);
            if (!matchExists)
                throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

            return await _lineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
        }

        public async Task DeletePlayerFromLineupAsync(int matchId, int lineupId)
        {
            // Validar que el partido existe y está en Scheduled antes de permitir modificar la alineación
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

            if (match.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException("Solo se pueden modificar alineaciones en partidos Scheduled");

            var existing = await _lineupRepository.GetByIdAsync(lineupId);
            if (existing == null || existing.MatchId != matchId)
                throw new KeyNotFoundException("No se encontró el registro en la alineación");

            await _lineupRepository.DeleteAsync(lineupId);
        }
    }
}