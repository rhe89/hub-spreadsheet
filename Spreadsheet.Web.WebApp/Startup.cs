using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hub.Shared.Storage.Repository;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spreadsheet.Data;
using Spreadsheet.Data.AutoMapper;
using Spreadsheet.Data.Dto;
using Spreadsheet.Providers;
using Spreadsheet.Services;
using Spreadsheet.Web.WebApp.Validation;

namespace Spreadsheet.Web.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddRazorPages();
            serviceCollection.AddServerSideBlazor(c => c.DetailedErrors = true);            
            serviceCollection.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
            {
                ConnectionString = Configuration.GetValue<string>("AI_CONNECTION_STRING")
            });
            
            serviceCollection.AddDatabase<SpreadsheetDbContext>(Configuration, "SQL_DB_SPREADSHEET", "Sbanken.Spreadsheet");
            serviceCollection.AddTransient<ISpreadsheetCosmosDb, SpreadsheetCosmosDb>();
            serviceCollection.AddTransient<ISpreadsheetMetadataProvider, SpreadsheetMetadataProvider>();
            serviceCollection.AddTransient<ISpreadsheetMetadataService, SpreadsheetMetadataService>();
            serviceCollection.AddSingleton<State>();
            
            serviceCollection.AddAutoMapper(c =>
            {
                c.AddSpreadsheetProfiles();
            });
 
            serviceCollection.AddFluentValidation();
            serviceCollection.AddTransient<IValidator<SpreadsheetMetadataDto>, SpreadsheetMetadataValidator>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}