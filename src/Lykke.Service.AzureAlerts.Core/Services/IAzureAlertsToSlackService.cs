using System.Threading.Tasks;
using Lykke.Service.AzureAlerts.Core.Domain.WebHooks;

namespace Lykke.Service.AzureAlerts.Core.Services
{
    /// <summary>
    /// Formats and send azure notifications to Slack
    /// </summary>
    public interface IAzureAlertsToSlackService
    {
        Task Send(AzureAlertNotification notification);
    }
}