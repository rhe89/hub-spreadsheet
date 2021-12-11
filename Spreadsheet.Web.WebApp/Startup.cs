using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hub.Shared.Logging;
using JetBrains.Annotations;
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
    [UsedImplicitly]
    public class Startup : Hub.Shared.Web.BlazorServer.Startup<DependencyRegistrationFactory>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}