using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationserviceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,IConfiguration config){
                    services.AddDbContext<DataContext>(options=>{
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                services.AddScoped<IToken,Token>();
            });
            return services;
        }
    }
}