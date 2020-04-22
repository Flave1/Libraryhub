using AutoMapper;
using Libraryhub.Data;
using Libraryhub.Domain;
using Libraryhub.DomainObjs;
using Libraryhub.Repository.Cache;
using Libraryhub.Service.ServiceImplementation;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Libraryhub.Installers
{
    public class DbInstaller : IInstaller
    { 
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                   options.UseSqlServer(
                       configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBookService, BookService>();
            //services.Decorate<IBookService, CachedBookService>();

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAutoMapper(typeof(Startup)); 
            services.AddMediatR(typeof(Startup));
            services.AddMvc();

        }
    }
}
