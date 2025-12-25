using Microsoft.EntityFrameworkCore;
using SilentAuction.Domain.Entities;

namespace SilentAuction.Infrastructure.Persistence
{
    public class SilentAuctionDbContext : DbContext
    {
        public SilentAuctionDbContext(DbContextOptions<SilentAuctionDbContext> options) : base(options)
        {
        }

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PostgreSQL'in kendi "xmin" (System Column) özelliğini kullan.
            // Bu sayede ekstra kolon oluşturmadan versiyon takibi yaparız.
            modelBuilder.Entity<Auction>()
                .UseXminAsConcurrencyToken();

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}