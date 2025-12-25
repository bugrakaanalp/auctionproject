using System;

namespace SilentAuction.Domain.Entities
{
    public class Bid
    {
        public Guid Id { get; set; } // Teklif ID'si

        // Hangi Müzayede için?
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }

        // Kim teklif verdi?
        public string UserId { get; set; }

        // Kaç para verdi?
        public decimal Amount { get; set; }

        // Ne zaman verdi?
        public DateTime CreatedAt { get; set; }
    }
}