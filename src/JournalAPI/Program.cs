using JournalAPI.Repositories;
using JournalAPI.Services.Journal;

namespace JournalAPI;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddScoped<ITestDataRepository, TestDataRepository>();
        builder.Services.AddSingleton<IJournalService,JournalService>();
        builder.Services.AddScoped<IJournalRepository, JournalRepository>();
        builder.Services.Configure<JournalOptions>(builder.Configuration.GetSection("Journal"));
        builder.Services.AddMemoryCache();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
