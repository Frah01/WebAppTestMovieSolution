using Serilog.Events;
using Serilog;
using WebAppTestMovieEF.Models;

namespace WebAppTestMovieEF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();   // NO Microsoft Logging - VEDERE Sezione Logging di appsettings.json

            // SERILOG
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)  // NO Microsoft Logging
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog(logger);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ADD ENTITY FRAMEWORK
            builder.Services.AddDbContext<CorsoAcademyContext>(
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
