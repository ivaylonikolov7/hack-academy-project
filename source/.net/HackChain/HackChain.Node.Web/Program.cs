using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace HackChain.Node.Web
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddAutoMapper(typeof(Program));

            services.AddDbContext<HackChainDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

            services.AddScoped<INodeService, NodeService>();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
           

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });

            app.Run();
        }
    }
}