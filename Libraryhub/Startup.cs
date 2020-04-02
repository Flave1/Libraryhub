 
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Libraryhub.Installers;
using Libraryhub.Logs;
using Microsoft.Extensions.Logging;
using System;
using Libraryhub.Extensions;
using System.Threading.Tasks;
using Libraryhub.Domain;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace Libraryhub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public IConfiguration Configuration { get; } 


        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
        } 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
           
            app.Use(async (ctx, next) => {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }

            });
            var loggerSetting = new Logging();
            Configuration.GetSection(nameof(Logging)).Bind(loggerSetting);
            if (loggerSetting.Enable) { loggerFactory.AddFile("Logs/flavehubLogs-{Date}.txt"); }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
        
            app.UseRouting();

            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
 
            app.UseSwaggerUI(option => {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseAuthentication();

            app.UseExceptionHandler("/errors/500");
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            CreateRolesAndAdminUser(serviceProvider).Wait();
        }









        
        private async Task CreateRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "User" }; 
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var userSettings = new UserSettings(); 
            Configuration.GetSection(nameof(UserSettings)).Bind(userSettings);

            var adminPassword = userSettings.Password; 
            var adminUser = new ApplicationUser()
            {
                Email = userSettings.Email,
                FullName = userSettings.FullName,
                UserName = userSettings.Email,
                PhoneNumber = userSettings.Phone
            };

            var user = await UserManager.FindByEmailAsync(userSettings.Email); 
            if(user == null)
            {
                var created = await UserManager.CreateAsync(adminUser, adminPassword); 
                if(created.Succeeded) { await UserManager.AddToRoleAsync(adminUser, "Admin"); } 
            }
        }
    }
}
