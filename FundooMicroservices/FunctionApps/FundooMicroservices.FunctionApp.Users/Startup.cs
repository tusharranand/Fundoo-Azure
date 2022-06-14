using FundooMicroservices.FunctionApp.Users;
using FundooMicroservices.Shared.Interfaces;
using FundooMicroservices.Shared.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FundooMicroservices.FunctionApp.Users
{
    public class Startup : FunctionsStartup
    {
        private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IUserServices, UserServices>();
            builder.Services.AddSingleton<ISettingServices, SettingServices>();
            builder.Services.AddSingleton<IJWTService, JWTService>();

        }
    }
}
