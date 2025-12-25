using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; 
using Microsoft.EntityFrameworkCore;
using SilentAuction.API.Hubs; 
using SilentAuction.Application.Interfaces;
using SilentAuction.Domain.Entities;

namespace SilentAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionRepository _repository;
        private readonly IHubContext<AuctionHub> _hubContext; // Yayın aracı

        // Constructor'da HubContext'i içeri alıyoruz
        public AuctionsController(IAuctionRepository repository, IHubContext<AuctionHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string productName, decimal startPrice, DateTime endDate)
        {
            var auction = new Auction
            {
                Id = Guid.NewGuid(),
                ProductName = productName,
                Description = "Lüks Ürün",
                StartingPrice = startPrice,
                StartsAt = DateTime.UtcNow,
                EndsAt = endDate.ToUniversalTime(),
                SellerId = "admin",
            };

            await _repository.AddAsync(auction);
            await _repository.SaveChangesAsync();
            return Ok(auction);
        }

        [HttpPost("{id}/bids")]
        public async Task<IActionResult> PlaceBid(Guid id, [FromQuery] decimal amount, [FromQuery] string userId)
        {
            try
            {
                var bid = new Bid
                {
                    Id = Guid.NewGuid(),
                    AuctionId = id,
                    UserId = userId,
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow
                };

                // 1. Veritabanına kaydet
                await _repository.PlaceBidAsync(bid);

                // "ReceiveBid" olayını dinleyen herkese bu mesaj gider.
                await _hubContext.Clients.All.SendAsync("ReceiveBid", userId, amount);

                return Ok(new { Message = "Teklif verildi ve yayınlandı!", NewPrice = amount });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Fiyat değişti! Lütfen sayfayı yenileyip tekrar deneyin." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
