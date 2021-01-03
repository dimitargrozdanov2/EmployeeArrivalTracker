using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Services;
using ReportingTool.Services.Contracts;
using ReportingTool.Web.Infrastructure;
using ReportingTool.Web.Services;
using ReportingTool.Web.Services.Http;

namespace ReportingTool.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpClient<IHttpClientService, HttpClientService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IArrivalService, ArrivalService>();
            services.AddScoped<IArrivalRepository, ArrivalRepository>();
            services.AddScoped<IServiceTokenRepository, ServiceTokenRepository>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseValidationExceptionHandler();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
