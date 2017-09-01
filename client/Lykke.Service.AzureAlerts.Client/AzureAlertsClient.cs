using System;
using Common.Log;

namespace Lykke.Service.AzureAlerts.Client
{
    public class AzureAlertsClient : IAzureAlertsClient, IDisposable
    {
        private readonly ILog _log;

        public AzureAlertsClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
