using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;
        }

        // GET: api/Sponsor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAllSponsors()
        {
            var sponsors = await _sponsorService.GetAllSponsorsAsync();
            var sponsorDtos = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
            return Ok(sponsorDtos);
        }

        // GET: api/Sponsor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetSponsorById(int id)
        {
            try
            {
                var sponsor = await _sponsorService.GetSponsorByIdAsync(id);
                return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/Sponsor
        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> CreateSponsor([FromBody] SponsorRequestDTO sponsorDto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(sponsorDto);
                var createdSponsor = await _sponsorService.CreateSponsorAsync(sponsor);
                var responseDto = _mapper.Map<SponsorResponseDTO>(createdSponsor);
                return CreatedAtAction(nameof(GetSponsorById), new { id = responseDto.Id }, responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict
            }
        }

        // PUT: api/Sponsor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSponsor(int id, [FromBody] SponsorRequestDTO sponsorDto)
        {
            try
            {
                var sponsor = _mapper.Map<Sponsor>(sponsorDto);
                await _sponsorService.UpdateSponsorAsync(id, sponsor);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Sponsor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSponsor(int id)
        {
            try
            {
                await _sponsorService.DeleteSponsorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // --- Endpoints para la relación N:M ---

        // POST: api/Sponsor/1/tournaments
        [HttpPost("{sponsorId}/tournaments")]
        public async Task<IActionResult> LinkSponsor(int sponsorId, [FromBody] LinkSponsorRequestDTO linkDto)
        {
            try
            {
                await _sponsorService.LinkSponsorToTournamentAsync(sponsorId, linkDto.TournamentId, linkDto.ContractAmount);
                // Retornar 201 Created es una opción, pero 200 OK también es común para acciones de vinculación.
                return Ok(new { message = "Patrocinador vinculado al torneo exitosamente." });
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

        // GET: api/Sponsor/1/tournaments
        [HttpGet("{sponsorId}/tournaments")]
        public async Task<ActionResult<IEnumerable<TournamentResponseDTO>>> GetLinkedTournaments(int sponsorId)
        {
            try
            {
                var tournaments = await _sponsorService.GetTournamentsBySponsorAsync(sponsorId);
                return Ok(_mapper.Map<IEnumerable<TournamentResponseDTO>>(tournaments));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Sponsor/1/tournaments/2
        [HttpDelete("{sponsorId}/tournaments/{tournamentId}")]
        public async Task<IActionResult> UnlinkSponsor(int sponsorId, int tournamentId)
        {
            try
            {
                await _sponsorService.UnlinkSponsorFromTournamentAsync(sponsorId, tournamentId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
