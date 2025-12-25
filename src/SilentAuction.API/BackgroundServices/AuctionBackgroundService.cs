using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SilentAuction.API.Hubs;
using SilentAuction.Infrastructure.Persistence;

namespace SilentAuction.API.BackgroundServices
{
    public class AuctionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<AuctionHub> _hubContext;
        private readonly ILogger<AuctionBackgroundService> _logger;

        public AuctionBackgroundService(
            IServiceProvider serviceProvider,
            IHubContext<AuctionHub> hubContext,
            ILogger<AuctionBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🤖 Müzayede Bekçi Robotu Başlatıldı...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Her turda yeni bir Scope (Kapsam) oluşturmalıyız
                    // Çünkü BackgroundService "Singleton" ama DbContext "Scoped"
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<SilentAuctionDbContext>();

                        // 1. Süresi dolmuş ama henüz "Bitti" denmemiş müzayedeleri bul
                        var finishedAuctions = await context.Auctions
                            .Where(a => a.EndsAt <= DateTime.UtcNow && !a.IsFinished)
                            .ToListAsync(stoppingToken);

                        foreach (var auction in finishedAuctions)
                        {
                            // 2. İşaretle
                            auction.IsFinished = true;

                            // 3. Kazananı belirle (Zaten CurrentHighestBid ve WinnerId var)
                            var winner = auction.WinnerId ?? "Kimse";
                            var price = auction.CurrentHighestBid ?? 0;

                            _logger.LogInformation($"🏁 Müzayede Bitti! ID: {auction.Id}, Kazanan: {winner}");

                            // 4. HERKESE DUYUR! (SignalR)
                            await _hubContext.Clients.All.SendAsync("AuctionEnded",
                                auction.Id,
                                winner,
                                price,
                                stoppingToken);
                        }

                        if (finishedAuctions.Any())
                        {
                            await context.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Bekçi Robot hata yaptı!");
                }

                // 5 saniye bekle ve tekrar kontrol et
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}