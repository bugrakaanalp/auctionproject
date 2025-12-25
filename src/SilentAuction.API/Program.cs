using SilentAuction.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- EKLEME 1: CORS Servisini Ekle (En üstlere yakýn) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .SetIsOriginAllowed(_ => true) // Her yerden gelene izin ver (localhost, dosya vs.)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // SignalR için bu zorunludur!
});
// -------------------------------------------------------

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // Döngüye giren nesneleri görmezden gel (Ignore Cycles)
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddSignalR();
builder.Services.AddHostedService<SilentAuction.API.BackgroundServices.AuctionBackgroundService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// DÝKKAT: UseCors BURADA olmalý (UseAuthorization'dan ÖNCE)
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
app.MapHub<SilentAuction.API.Hubs.AuctionHub>("/auctionHub");

app.Run();