using System.ComponentModel.DataAnnotations;

namespace SportsLeague.API.DTOs.Request
{
    public class CreateMatchLineupDto
    {
        [Required(ErrorMessage = "El jugador es obligatorio.")]
        public int PlayerId { get; set; }

        [Required]
        public bool IsStarter { get; set; }

        [Required(ErrorMessage = "La posición es obligatoria.")]
        [MaxLength(10)]
        public string Position { get; set; } = string.Empty; // Ej: "GK", "CB", "ST"
    }
}