using FundooMicroservices.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Services
{
    public class SettingServices : ISettingServices
    {
        // Cosmos DB
        private const string DocDbEndpointUriKey = "DocDbEndpointUri";
        private const string DocDbApiKey = "DocDbApiKey";
        private const string DocDbConnectionStringKey = "DocDbConnectionStringKey";
        private const string DocDbDatabaseNameKey = "DocDbDatabaseName";
        private const string DocDbMainCollectionNameKey = "DocDbMainCollectionName";
        private const string DocDbThroughput = "DocDbThroughput";

        public string GetDocDbEndpointUri()
        {
            return GetEnvironmentVariable(DocDbEndpointUriKey);
        }

        public string GetDocDbApiKey()
        {
            return GetEnvironmentVariable(DocDbApiKey);
        }

        public string GetDocDbConnectionString()
        {
            return GetEnvironmentVariable(DocDbConnectionStringKey);
        }

        public string GetDocDbDatabaseName()
        {
            return GetEnvironmentVariable(DocDbDatabaseNameKey);
        }

        public string GetDocDbMainCollectionName()
        {
            return GetEnvironmentVariable(DocDbMainCollectionNameKey);
        }

        public int? GetDocDbThroughput()
        {
            if (int.TryParse(GetEnvironmentVariable(DocDbThroughput), out int throughput)) return throughput;
            return null;
        }

        private static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
