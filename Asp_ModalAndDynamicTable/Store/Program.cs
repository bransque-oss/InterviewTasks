using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Services;

namespace Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbConnection"));
            });

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}