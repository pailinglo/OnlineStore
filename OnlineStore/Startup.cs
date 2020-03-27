using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineStore.Models;
using OnlineStore.Security;
using OnlineStore.Services;

namespace OnlineStore
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
            services.AddRazorPages();
            //use AddDbContextPool rather than AddDbContext
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("InventoryDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.SignIn.RequireConfirmedEmail = true;
                        //maximum failed attempt before locked out:
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    }

                )  //using Microsoft.AspNetCore.Identity
                .AddEntityFrameworkStores<AppDbContext>()      //adds an entity framework implementation of identity informaiton stores
                .AddDefaultTokenProviders();                   //to generate token for user e-mail confirmation.


            //to use policy based authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
                
                //This piece of code has same effect as code below.
                //options.AddPolicy("DeleteRolePolicy", policy => policy.RequireAssertion(
                //    context => context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                //    && context.User.HasClaim(claim => claim.Type == "Email" && claim.Value == "fly@fly.com")));


                //user must have Edit Role claim and has specified e-mail for this policy
                options.AddPolicy("DeleteRolePolicy", 
                    policy => policy.RequireClaim("Email", new string[] { "fly@fly.com", "super@fly.com" })
                                    .RequireClaim("Edit Role"));


            });
            //for session to work
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//We set Time here 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // for custom components that require access to HttpContext
            services.AddHttpContextAccessor();

            services.AddScoped<IProductRepository, SQLProductRepository>();
            services.AddScoped<ICategoryRepository, SQLCategoryRepository>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();
            services.AddScoped<IShoppingCart, ShoppingCart>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //for session to work.
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
