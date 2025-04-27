using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Dynamic;
using System.Security.Cryptography;
using aa.Models;
using aa.Controllers;
using System.Configuration;

namespace aa;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SoupDbContext>(options =>
       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "client/src";
        });
        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        return services;
    }
}
