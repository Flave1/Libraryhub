using System;
using System.Linq;
using System.Threading.Tasks;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.V1;
using Libraryhub.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libraryhub.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationReqObj regRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                var authResponse = await _identityService.RegisterAsync(regRequest);
                if (!authResponse.Success)
                {
                    return BadRequest(new AuthFailedResponse
                    {
                        Errors = authResponse.Errors
                    });
                }

                return Ok(new AuthSuccessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken
                });
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginReqObj request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Identity.RefreshToken)]
        public async Task<IActionResult> Refresh([FromBody] UserRefreshTokenReqObj request)
        {

            var authResponse = await _identityService.RefreshTokenAsync(request.RefreshToken, request.Token);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

    }
}