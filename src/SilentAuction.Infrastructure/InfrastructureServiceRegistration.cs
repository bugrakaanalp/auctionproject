using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentAuction.Application.Interfaces;
using SilentAuction.Infrastructure.Persistence;
using SilentAuction.Infrastructure.Repositories;

namespace SilentAuction.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        // Bu metot, API katmanından çağrılacak ve servisleri yükleyecek.
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Veritabanı bağlantı cümlesini (Connection String) alıyoruz.
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // DbContext'i servis havuzuna ekliyoruz.
            services.AddDbContext<SilentAuctionDbContext>(options =>
                options.UseNpgsql(connectionString));
            // Repository'i servise kaydediyoruz (Scoped: Her istekte yeni bir tane oluşur)
            services.AddScoped<IAuctionRepository, AuctionRepository>();

            return services;
        }
    }
}