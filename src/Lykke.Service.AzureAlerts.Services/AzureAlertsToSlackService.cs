using System.Threading.Tasks;
using Lykke.Service.AzureAlerts.Core.Domain;
using Lykke.Service.AzureAlerts.Core.Domain.WebHooks;
using Lykke.Service.AzureAlerts.Core.Services;
using Lykke.SlackNotifications;

namespace Lykke.Service.AzureAlerts.Services
{
    public sealed class AzureAlertsToSlackService : IAzureAlertsToSlackService
    {
        private readonly ISlackNotificationsSender _slackNotificationsSender;

        public AzureAlertsToSlackService(ISlackNotificationsSender slackNotificationsSender)
        {
            _slackNotificationsSender = slackNotificationsSender;
        }

        public async Task Send(AzureAlertNotification notification)
        {
            var sender = notification.Context.ResourceName;
            var msg = FormatMessage(notification);
            await _slackNotificationsSender.SendAsync(Const.SlackMsgType, sender, msg);
        }

        private static string FormatMessage(AzureAlertNotification notification)
        {
            var cond = notification.Context.Condition;
            return $"Status\t{notification.Status}.\t{cond.MetricName} value is\t{cond.MetricValue} {cond.MetricUnit}\t{cond.Operator}\t{cond.Threshold} {cond.MetricUnit}";
        }
    }
}