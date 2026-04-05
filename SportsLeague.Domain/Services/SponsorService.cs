using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
        }

        // --- CRUD Methods ---
        public async Task<Sponsor> CreateSponsorAsync(Sponsor sponsor)
        {
            // Validación: Nombre duplicado
            if (await _sponsorRepository.ExistsByNameAsync(sponsor.Name))
            {
                throw new InvalidOperationException("Ya existe un patrocinador con este nombre.");
            }

            // Validación: Email válido
            if (!new EmailAddressAttribute().IsValid(sponsor.ContactEmail))
            {
                throw new InvalidOperationException("El formato del email de contacto no es válido.");
            }

            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task DeleteSponsorAsync(int id)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
            {
                throw new KeyNotFoundException("Patrocinador no encontrado.");
            }
            await _sponsorRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<Sponsor>> GetAllSponsorsAsync()
        {
            return _sponsorRepository.GetAllAsync();
        }

        public Task<Sponsor?> GetSponsorByIdAsync(int id)
        {
            var sponsor = _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
            {
                throw new KeyNotFoundException("Patrocinador no encontrado.");
            }
            return sponsor;
        }

        public async Task UpdateSponsorAsync(int id, Sponsor sponsor)
        {
            var existingSponsor = await _sponsorRepository.GetByIdAsync(id);
            if (existingSponsor == null)
            {
                throw new KeyNotFoundException("Patrocinador no encontrado.");
            }

            // ... (aquí irían más validaciones si se necesitaran para el update)

            existingSponsor.Name = sponsor.Name;
            existingSponsor.ContactEmail = sponsor.ContactEmail;
            existingSponsor.Phone = sponsor.Phone;
            existingSponsor.WebsiteUrl = sponsor.WebsiteUrl;
            existingSponsor.Category = sponsor.Category;

            await _sponsorRepository.UpdateAsync(existingSponsor);
        }

        // --- Linking Methods ---
        public async Task LinkSponsorToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            // Validación: ContractAmount > 0
            if (contractAmount <= 0)
            {
                throw new InvalidOperationException("El monto del contrato debe ser mayor a cero.");
            }

            // Validación: Sponsor existe
            if (!await _sponsorRepository.ExistsAsync(sponsorId))
            {
                throw new KeyNotFoundException("El patrocinador especificado no existe.");
            }

            // Validación: Tournament existe
            if (!await _tournamentRepository.ExistsAsync(tournamentId))
            {
                throw new KeyNotFoundException("El torneo especificado no existe.");
            }

            // Validación: Vínculo duplicado
            var existingLink = await _tournamentSponsorRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
            if (existingLink != null)
            {
                throw new InvalidOperationException("Este patrocinador ya está vinculado a este torneo.");
            }

            var newLink = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };

            await _tournamentSponsorRepository.CreateAsync(newLink);
        }

        public async Task UnlinkSponsorFromTournamentAsync(int sponsorId, int tournamentId)
        {
            var existingLink = await _tournamentSponsorRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
            if (existingLink == null)
            {
                throw new KeyNotFoundException("El vínculo entre este patrocinador y torneo no existe.");
            }

            await _tournamentSponsorRepository.DeleteAsync(existingLink.Id);
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            if (!await _sponsorRepository.ExistsAsync(sponsorId))
            {
                throw new KeyNotFoundException("El patrocinador especificado no existe.");
            }

            var links = await _tournamentSponsorRepository.GetTournamentsBySponsorIdAsync(sponsorId);
            return links.Select(l => l.Tournament);
        }
    }
}
