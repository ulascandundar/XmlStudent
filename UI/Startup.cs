using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace UI
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
            services.AddSignalR();
            services.Configure<AppSettings>(Configuration);
            services.AddControllersWithViews()
                   .AddJsonOptions(o =>
                   {
                       o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                       o.JsonSerializerOptions.PropertyNamingPolicy = null;
                   });
            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDependencyResolvers(new ICoreModule[] { new CoreModule() });
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(x =>
                {
                    x.LoginPath = "/Login/Index";
                });
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                               .RequireAuthenticatedUser()
                               .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.sig(route =>
            //{
            //    route.MapHub<ChatHub>("/chathub");
            //});
            //app.UseEndpoints(endpoints =>
            //{

            //    endpoints.MapHub<ChatHub>("/chatHub");
            //});


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                     template: "{controller=Student}/{action=Index}/{id?}");
            });
        }
    }
}
