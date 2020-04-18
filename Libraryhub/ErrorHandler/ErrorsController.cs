using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Libraryhub.CustomError
{
    [Route("/errors")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        private readonly ILogger _logger;
        public ErrorsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(ErrorsController));
        }
        [Route("{code}")]
        public IActionResult GenerateErrorMessage(int? code)
        {
            BaseApiError error;
            HttpContext context = HttpContext;
            HttpStatusCode parseCode = (HttpStatusCode)code;

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonConvert.SerializeObject(exception.Message);

                context.Response.ContentType = "application/json";

                if (parseCode == HttpStatusCode.NotFound)
                {
                    error = new BaseApiError(code ?? 0, parseCode.ToString(), "Use A Valid URL");
                    _logger.LogInformation("checkError", error.Technical_Message);
                    return new ObjectResult(error);
                }
                if (parseCode == HttpStatusCode.InternalServerError)
                {
                    error = new BaseApiError(code ?? 0, parseCode.ToString(), "Unable to process Request! Please try again later, The server encountered an internal error or misconfiguration ", result);
                    _logger.LogInformation("checkError", error.Technical_Message);
                    return new ObjectResult(error);
                }

                if (parseCode == HttpStatusCode.Forbidden)
                {
                    error = new BaseApiError(code ?? 0, parseCode.ToString(), "You do not have enough permission to perform operation", result);
                    _logger.LogInformation("checkError", error.Technical_Message);
                    return new ObjectResult(error);
                }

                error = new BaseApiError(code ?? 0, parseCode.ToString(), result);
                _logger.LogInformation("checkError", error.Technical_Message);
                return new ObjectResult(error);
            }
            if (parseCode == HttpStatusCode.NotFound)
            {
                error = new BaseApiError(code ?? 0, parseCode.ToString(), "Use A Valid URL");
                _logger.LogInformation("checkError", error.Technical_Message);
                return new ObjectResult(error);
            }
            error = new BaseApiError(code ?? 0, parseCode.ToString());
            return new ObjectResult(error);
        }




    }
}
