using HackChain.Core.Interfaces;
using HackChain.Core.Services;
using System.Configuration;

namespace HackChain.Node.Web
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INodeService, NodeService>();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            ConfigureServices(builder.Services);

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