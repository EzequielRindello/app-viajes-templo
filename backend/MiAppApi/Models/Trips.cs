using System;
using System.Collections.Generic;

namespace MiAppApi.Models
{
    public class Trips
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalSeats { get; set; }
        public int SeatsAvailable { get; set; }
        public decimal CostPerPerson { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
