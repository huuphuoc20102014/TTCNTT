using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ATAdmin.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ATAdmin.Efs.Entities;
using FluentValidation.AspNetCore;
using ATAdmin.Models;

namespace ATAdmin
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
            services.AddKendo();
            services.AddControllersWithViews()
             .AddNewtonsoftJson(options =>
             {

                 // Return JSON responses in LowerCase?

                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                 // Resolve Looping navigation properties

                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             });
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.Configure<StaticFileSetting>(Configuration.GetSection("StaticFileSetting"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("WebAtSolutionContext")));

            services.AddDbContext<WebAtSolutionContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("WebAtSolutionContext")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();



            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
               .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AtRegisterValidator>())
               .AddJsonOptions(o =>
               {
                   o.JsonSerializerOptions.PropertyNamingPolicy = null;
                   o.JsonSerializerOptions.DictionaryKeyPolicy = null;
               })
               .AddViewLocalization(); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               //Key -> StaticFileSetting || Value -> "D:\\ATImage"
               //Configuration.GetSection("StaticFileSetting").Value -> "E:\\ATImage"
                Path.Combine(Configuration.GetSection("StaticFileSetting").Value)),
                RequestPath = "/Image"
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
