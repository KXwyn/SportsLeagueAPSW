using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request
{
    public class LinkSponsorRequestDTO
    {
        [Required]
        public int TournamentId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ContractAmount { get; set; }
    }
}
