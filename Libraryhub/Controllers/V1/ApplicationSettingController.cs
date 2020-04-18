using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Contracts.V1;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Controllers.V1
{
    public class ApplicationSettingController : Controller
    {
        private readonly IAppSettingService _appSettingService;
        private readonly ILogger _logger;
        public ApplicationSettingController(IAppSettingService appSettingService, ILoggerFactory loggerFactory)
        {
            _appSettingService = appSettingService;
            _logger = loggerFactory.CreateLogger(typeof(ApplicationSettingController));
        }
        [HttpPut(ApiRoutes.Settings.CHECKOUT_REMINDER_ENDPOINT)]
        public async Task<ActionResult<EmailResponseObj>> CheckoutReminder(int SwitchValue)
        {
            try
            {
                var done = await _appSettingService.CheckoutReminder(SwitchValue);
                return new EmailResponseObj
                {
                    IsSuccessful = done ? true : false,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = done ? true : false,
                    }
                };
            }
            catch (Exception ex)
            {
                var errorId = ErrorID.Generate(4);
                _logger.LogInformation($"ApplicationSettingController{errorId}", $"Error Message{ ex.InnerException.Message}");
                return new EmailResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Something went wrong",
                            MessageId = $"ApplicationSettingController{errorId}",
                            TechnicalMessage = ex.InnerException.Message
                        }
                    }
                };
            }
             
        }
    }
}
