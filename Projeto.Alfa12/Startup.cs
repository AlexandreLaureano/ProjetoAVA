﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;
using Projeto.Alfa12.Services;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Projeto.Alfa12
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
         //   AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //iframe funcionar
            //  services.AddAntiforgery(opts => { opts.SuppressXFrameOptionsHeader = true; });
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        
                    }
                    );
                
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

           
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
            app.UseStaticFiles();

            app.UseAuthentication();


            app.UseCors("AllowAll");


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: null,
                template: "Log/List/{category}/Page{productPage:int}",
                defaults: new { controller = "LogUsuarios", action = "List" });

                routes.MapRoute(
                name: null,
                template: "Log/List/Page{productPage:int}",
                defaults: new{ controller = "LogUsuarios", action = "List", productPage = 1});

                routes.MapRoute(
                name: null,
                template: "Log/List/{category}",
                defaults: new{    controller = "LogUsuarios", action = "List", productPage = 1 } );

                routes.MapRoute(
                name: null,
                template: "Log/List",
                defaults: new{ controller = "LogUsuarios", action = "List",productPage = 1 });

                routes.MapRoute(
                 name: "pagination",
                 template: "{Controller}/List/Page{productPage}",
                 defaults: new { Controller = "Turmas", action = "List" });

                routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");


            });

        }
    }
}
