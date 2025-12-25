using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SilentAuction.API.Hubs
{
    // Bu sınıf, bağlı olan tüm kullanıcıları yönetir.
    public class AuctionHub : Hub
    {
        // İstemciler (Frontend) bu metoda abone olacak.
        public async Task SendNewBid(string user, decimal amount)
        {
            // "ReceiveBid" kanalını dinleyen herkese mesaj gönder
            await Clients.All.SendAsync("ReceiveBid", user, amount);
        }
    }
}