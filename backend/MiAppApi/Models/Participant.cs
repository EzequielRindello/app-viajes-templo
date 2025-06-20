using System;
using System.ComponentModel.DataAnnotations;

namespace MiAppApi.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public int TripId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public bool PaymentComplete { get; set; } = false;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Trips? Trip { get; set; }
    }
}