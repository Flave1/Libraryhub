using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.V1;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Controllers.V1
{
    public class OrderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUriService _uriService;
        public OrderController(IMediator mediator, IMapper mapper, IUriService uriService)
        {
            _uriService = uriService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost(ApiRoutes.Order.ORDER_ENDPOINT)]
        public async Task<ActionResult<RegBookResponseObj>> CheckInBook([FromBody] OrderObj requestObj)
        {
            OrderCommand command = _mapper.Map<OrderCommand>(requestObj);
            var response = await _mediator.Send(command);
            if (response.BookId < 1 || !response.Status.IsSuccessful)
            {
                return BadRequest(response);
            }
            var locatioUri = _uriService.GetBookUri(response.BookId.ToString());
            return Created(locatioUri, response);
        }
    }
}
