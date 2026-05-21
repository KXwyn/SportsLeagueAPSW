using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/match/{matchId}/lineup")]
    public class MatchLineupController : ControllerBase
    {
        private readonly IMatchLineupService _matchLineupService;
        private readonly IMapper _mapper;

        public MatchLineupController(IMatchLineupService matchLineupService, IMapper mapper)
        {
            _matchLineupService = matchLineupService;
            _mapper = mapper;
        }

        // POST /api/match/{matchId}/lineup
        [HttpPost]
        public async Task<ActionResult<MatchLineupResponseDto>> AddPlayerToLineup(int matchId, [FromBody] CreateMatchLineupDto dto)
        {
            try
            {
                var lineupEntity = _mapper.Map<MatchLineup>(dto);
                var created = await _matchLineupService.AddPlayerToLineupAsync(matchId, lineupEntity);
                var response = _mapper.Map<MatchLineupResponseDto>(created);
                return Created("", response); // 201 Created
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409
            }
        }

        // GET /api/match/{matchId}/lineup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDto>>> GetFullLineup(int matchId)
        {
            try
            {
                var lineup = await _matchLineupService.GetLineupAsync(matchId);
                var response = _mapper.Map<IEnumerable<MatchLineupResponseDto>>(lineup);
                return Ok(response); // 200
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET /api/match/{matchId}/lineup/team/{teamId}
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDto>>> GetTeamLineup(int matchId, int teamId)
        {
            try
            {
                var lineup = await _matchLineupService.GetTeamLineupAsync(matchId, teamId);
                var response = _mapper.Map<IEnumerable<MatchLineupResponseDto>>(lineup);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE /api/match/{matchId}/lineup/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerFromLineup(int matchId, int id)
        {
            try
            {
                await _matchLineupService.DeletePlayerFromLineupAsync(matchId, id);
                return NoContent(); // 204
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}