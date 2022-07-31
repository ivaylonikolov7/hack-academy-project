using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Core.Services;
using HackChain.Node.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace HackChain.Node.Web
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
                options.Filters.Add(typeof(HackChainExceptionFilterAttribute))
            );
            services.AddRazorPages();
            services.AddAutoMapper(typeof(Program));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "HackChain Node",
                    Description = "Hack Academy Group project."
                });
            });

            services.AddDbContext<HackChainDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

            services.AddScoped<INodeService, NodeService>();
            services.AddScoped<ITransactionService, TransactionService>();

            var settings = new HackChainSettings();
            configuration.Bind("HackChainSettings", settings);
            services.AddSingleton(settings);

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.MapRazorPages();
            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });

            app.Run();
        }
    }
}