using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Models;

namespace UserManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:UserManagementSystemDbConnection"]));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters += " ";

            }).AddEntityFrameworkStores<AppDbContext>();
            services.AddAuthentication();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "AppAuthorizationCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/account/login";
            });
            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.Zero);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllerRoute(name: "Default", pattern: "{Controller=Home}/{Action=Index}/{id?}"));
        }

        public IConfiguration Configuration { get; }
    }
}
