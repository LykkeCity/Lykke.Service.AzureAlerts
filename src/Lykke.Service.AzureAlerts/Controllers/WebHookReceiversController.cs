// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.AzureAlerts.Core;
using Lykke.Service.AzureAlerts.Core.Domain.WebHooks;
using Lykke.Service.AzureAlerts.Core.Services;
using Lykke.Service.AzureAlerts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.AzureAlerts.Controllers
{
    [Route("api/webhooks/incoming")]
    public sealed class WebHookReceiversController : Controller
    {
        private readonly AzureAlertsSettings _settings;
        private readonly ILog _log;
        private readonly IAzureAlertsToSlackService _sendService;

        public WebHookReceiversController(AzureAlertsSettings settings, ILog log, IAzureAlertsToSlackService sendService)
        {
            _settings = settings;
            _log = log;
            _sendService = sendService;
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation("Post")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> Post([FromBody]AzureAlertNotification notification, [FromQuery] string code)
        {
            return ProcessWebHook(notification, code);
        }

        private async Task<IActionResult> ProcessWebHook(AzureAlertNotification notification, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest(ErrorResponse.Create("The WebHook verification request must contain a 'code' query parameter."));
            }

            if (notification == null)
            {
                return BadRequest(ErrorResponse.Create("The request has no content"));
            }

            try
            {
                if (code != _settings.Secret)
                {
                    const string msg = "The code query parameter provided in the HTTP request did not match the expected value.";
                    await _log.WriteWarningAsync(nameof(WebHookReceiversController), nameof(ProcessWebHook), "Executing", msg);
                    return BadRequest(ErrorResponse.Create(msg));

                }
                await _sendService.Send(notification);
                return Ok();
            }
            catch (Exception rex)
            {
                await _log.WriteWarningAsync(nameof(WebHookReceiversController), nameof(ProcessWebHook), "Executing", rex.Message);

                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    ErrorResponse.Create(rex.Message));
            }
        }
    }
}
