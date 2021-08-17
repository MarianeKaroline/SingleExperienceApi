using SingleExperience.Repository.Services.BoughtServices;
using SingleExperience.Services.EmployeeServices;
using SingleExperience.Services.ProductServices;
using Microsoft.Extensions.DependencyInjection;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.UserServices;
using Microsoft.Extensions.Configuration;
using SingleExperience.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SingleExperience.Domain;

namespace SingleExperience.WebAPI
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
            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection"));
            });

            services.AddScoped<Session>();
            services.AddScoped<ProductService>();
            services.AddScoped<BoughtService>();
            services.AddScoped<CartService>();
            services.AddScoped<ClientService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<UserService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
