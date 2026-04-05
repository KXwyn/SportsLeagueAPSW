namespace SportsLeague.Domain.Entities
{
    public class TournamentSponsor : AuditBase
    {
        public int TournamentId { get; set; }
        public int SponsorId { get; set; }
        public decimal ContractAmount { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation Properties
        public virtual Tournament Tournament { get; set; } = null!;
        public virtual Sponsor Sponsor { get; set; } = null!;
    }
}
