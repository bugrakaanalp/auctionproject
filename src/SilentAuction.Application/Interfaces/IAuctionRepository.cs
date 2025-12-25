using SilentAuction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilentAuction.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task<List<Auction>> GetAllAsync();
        Task<Auction?> GetByIdAsync(Guid id); // '?' eklendi, null dönebilir
        Task AddAsync(Auction auction);

        // YENİ: Teklif verme metodu
        Task PlaceBidAsync(Bid bid);

        Task SaveChangesAsync();
    }
}