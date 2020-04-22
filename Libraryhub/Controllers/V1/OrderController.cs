using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.Queries;
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

        [HttpPost(ApiRoutes.Order.ORDER_DETAILS_ENDPOINT)]
        public async Task<ActionResult<OrderDetailResponseObj>> GetAllDetails()
        {
            var query = new OrderDetailQuery();
            return await _mediator.Send(query);  
        }


        [HttpPost(ApiRoutes.Order.SEARCH_ORDER_DETAILS_ENDPOINT)]
        public async Task<ActionResult<OrderDetailResponseObj>> OrderDetailsSearch([FromBody] OrderDetailsSearch req)
        {
            OrderDetailsSearchCommand command = _mapper.Map<OrderDetailsSearchCommand>(req); 
            return await _mediator.Send(command);
        }

        [HttpPost(ApiRoutes.Order.ORDER_ENDPOINT)]
        public async Task<ActionResult<RegBookResponseObj>> CheckInBook([FromBody] OrderObj requestObj)
        {
            OrderCommand command = _mapper.Map<OrderCommand>(requestObj);
            var response = await _mediator.Send(command);
            if (response.OrderId < 1 || !response.Status.IsSuccessful)
            {
                return BadRequest(response);
            }
            var locatioUri = _uriService.GetBookUri(response.OrderId.ToString());
            return Created(locatioUri, response);
        }

        [HttpPost(ApiRoutes.Order.CONFIRM_ORDER_ENDPOINT)]
        public async Task<ActionResult<RegBookResponseObj>> ConfirmOrder([FromBody] ConfirmOrder requestObj)
        {
            ConfirmOrderCommand command = _mapper.Map<ConfirmOrderCommand>(requestObj);
            var response = await _mediator.Send(command);
            if (response.OrderId < 1 || !response.Status.IsSuccessful)
            {
                return BadRequest(response);
            }
            var locatioUri = _uriService.GetBookUri(response.OrderId.ToString());
            return Created(locatioUri, response);
        }
    }
}
