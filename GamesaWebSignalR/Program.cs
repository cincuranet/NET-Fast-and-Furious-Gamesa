using Gamesa.Shared;
using GamesaWebSignalR.Hubs;
using QRCoder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddSingleton<IGameStore, GameStore>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors(builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
});
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapHub<GameHub>("/game-hub");
app.MapPost("/initialize", async (InitializeGameData data, IGameStore gameStore) =>
{
    var game = await gameStore.TryGetGameAsync(data.Id);
    if (game == null)
    {
        await gameStore.InitializeNewGameAsync(data.Id, data.Count);
    }
});
app.MapGet("/list", async (IGameStore gameStore) =>
{
    return await gameStore.ListGamesAsync();
});
app.MapGet("/qr/{id:alpha}", (string id, HttpRequest request) =>
{
    using (var generator = new QRCodeGenerator())
    {
        using (var data = generator.CreateQrCode($"{Config.ServerAddress}/{id}", QRCodeGenerator.ECCLevel.Q))
        {
            using (var code = new AsciiQRCode(data))
            {
                return code.GetGraphic(1);
            }
        }
    }
});
app.Run();
