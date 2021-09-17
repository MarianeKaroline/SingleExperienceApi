using SingleExperience.Repository.Services.BoughtServices;
using SingleExperience.Services.EmployeeServices;
using SingleExperience.Services.ProductServices;
using Microsoft.Extensions.DependencyInjection;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.UserServices;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SingleExperience.Domain;
using SingleExperience.Repository.Services.EmailServices.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SingleExperience.Repository.Services.EmailServices;

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
            services.AddCors();

            var appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);
            var jwtSettings = appSettings.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection"));
            });

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailModel>();

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

            app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
