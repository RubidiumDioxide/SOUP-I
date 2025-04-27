using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using System;
using System.Dynamic;
using System.Security.Cryptography;
using aa.Models;
using aa.Controllers;
using System.Configuration;

namespace aa;

public class Program
{
    public static void Main(string[] args)
    {
        var app_builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        app_builder.Services.AddMyServices(app_builder.Configuration); 
     
        var app = app_builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();

        var spaPath = "/app";
        if (app.Environment.IsDevelopment())
        {
            app.MapWhen(y => y.Request.Path.StartsWithSegments(spaPath), client =>
            {
                client.UseSpa(spa =>
                {
                    spa.UseProxyToSpaDevelopmentServer("https://localhost:6363");
                });
            });
        }
        else
        {
            app.Map(new PathString(spaPath), client =>
            {
                client.UseSpaStaticFiles();
                client.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "client";
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                    {
                        OnPrepareResponse = ctx =>
                        {
                            ResponseHeaders headers = ctx.Context.Response.GetTypedHeaders();
                            headers.CacheControl = new CacheControlHeaderValue
                            {
                                NoCache = true,
                                NoStore = true,
                                MustRevalidate = true
                            };
                        }
                    };
                });
            });
        }

        var serviceProvider = app_builder.Services.BuildServiceProvider();

        using (var dbContext = serviceProvider.GetRequiredService<SoupDbContext>())
        {
            app.Run();
        }
    }
}




