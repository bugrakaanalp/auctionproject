using System;
using System.Collections.Generic;

namespace SilentAuction.Domain.Entities
{
    public class Auction
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? CurrentHighestBid { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }

        public bool IsFinished { get; set; }

        public string SellerId { get; set; }
        public string? WinnerId { get; set; }

        public bool IsActive => DateTime.UtcNow >= StartsAt && DateTime.UtcNow <= EndsAt;

        public ICollection<Bid> Bids { get; set; }

        public Auction()
        {
            Bids = new List<Bid>();
        }
    }
}