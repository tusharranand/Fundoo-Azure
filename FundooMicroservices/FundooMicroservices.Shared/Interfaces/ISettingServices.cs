using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Interfaces
{
    public interface ISettingServices
    {
        // Cosmos
        string GetDocDbEndpointUri();
        string GetDocDbApiKey();
        string GetDocDbConnectionString();
        string GetDocDbDatabaseName();
        string GetDocDbMainCollectionName();
        int? GetDocDbThroughput();
    }
}
