using SportsLeague.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request
{
    public class SponsorRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }

        [Required]
        public SponsorCategory Category { get; set; }
    }
}
