using Microsoft.EntityFrameworkCore;
using SilentAuction.Application.Interfaces;
using SilentAuction.Domain.Entities;
using SilentAuction.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilentAuction.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly SilentAuctionDbContext _context;

        public AuctionRepository(SilentAuctionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auction>> GetAllAsync()
        {
            return await _context.Auctions
                .Include(x => x.Bids)
                .ToListAsync();
        }

        public async Task<Auction?> GetByIdAsync(Guid id)
        {
            return await _context.Auctions
                .Include(x => x.Bids)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
        }

        // MÜLAKAT CEVABI BURADA:
        public async Task PlaceBidAsync(Bid bid)
        {
            // 1. Müzayedeyi bul
            var auction = await _context.Auctions.FindAsync(bid.AuctionId);
            if (auction == null) throw new Exception("Müzayede bulunamadı!");

            // 2. Kural kontrolleri
            if (DateTime.UtcNow > auction.EndsAt)
                throw new Exception("Müzayede süresi dolmuş!");

            if (bid.Amount <= (auction.CurrentHighestBid ?? auction.StartingPrice))
                throw new Exception("Teklif, mevcut fiyattan yüksek olmalı!");

            // 3. Teklifi ekle
            await _context.Bids.AddAsync(bid);

            // 4. Müzayedenin fiyatını güncelle
            // (Burası Auction tablosunu güncellediği için xmin değişecek)
            auction.CurrentHighestBid = bid.Amount;
            auction.WinnerId = bid.UserId;

            // 5. Kaydet (Eğer biz işlem yaparken başkası fiyatı değiştirdiyse burada HATA fırlatır)
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}