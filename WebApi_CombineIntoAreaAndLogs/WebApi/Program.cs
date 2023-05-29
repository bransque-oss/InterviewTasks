using Data;
using Microsoft.EntityFrameworkCore;
using Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddDbContext<WarehouseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CoalWarehouseDatabase"));
            });
            builder.Services.AddScoped<CoalWarehouseService, CoalWarehouseService>();

            var app = builder.Build();

            app.MapControllers();

            app.Run();
        }
    }
}