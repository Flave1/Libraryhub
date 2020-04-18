using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Contracts.V1;
using Libraryhub.CustomError;
using Libraryhub.Data;
using Libraryhub.ErrorHandler;
using Libraryhub.MailHandler;
using Libraryhub.MailHandler.Service;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Libraryhub.Controllers.V1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUriService _uriService;
        public BookController(IMediator mediator, IMapper mapper, IUriService uriService)
        {
            _uriService = uriService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Book.LOAD_ALL_BOOKS_ENDPOINT)]
        public async Task<ActionResult<IList<BookResponseObj>>> GetAllPostAsync()
        { 
            var query = new GetAllBooksQuery();
            var response = await _mediator.Send(query); 
            return Ok(response);
        }
        
        [HttpPost(ApiRoutes.Book.BOOK_SEARCH_ENDPOINT)]
        public async Task<ActionResult<BookResponseObj>> Search([FromBody] BookSearchObj command)
        {
            var response = await _mediator.Send(command);
            return response != null ? (ActionResult)Ok(response) : NotFound(response);
        }

        [HttpPost(ApiRoutes.Book.GET_ALL_BOOK_PEANALTY_CHARGIES_ENDPOINT)]
        public async Task<ActionResult<IList<PenaltyChargeResponseObj>>> GetAllBookPenaltyChargies()
        {
            var query = new GetAllPenaltyChargeQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost(ApiRoutes.Book.GET_CUSTOMER_PENALTY_CHARGIES_ENDPOINT)]
        public async Task<IActionResult> GetCustomerPenaltyChargies([FromBody] CustomerPenaltyChargeSearchObj req)
        {
            var query = new GetCustomerPenaltyChargiesQuery(req.CustomerId);
            var response = await _mediator.Send(query);
            return response != null ? (IActionResult)Ok(response) : NotFound(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Book.CREATE_BOOK_ENDPOINT)]
        public async Task<ActionResult<RegBookResponseObj>> Create([FromBody] AddBookRequestObj requestObj)
        {
            AddBookCommand command = _mapper.Map<AddBookCommand>(requestObj);
            var response = await _mediator.Send(command);
            if (response.BookId < 1 || !response.Status.IsSuccessful)
            {
                return BadRequest(response);
            }
            var locatioUri = _uriService.GetBookUri(response.BookId.ToString());
            return Created(locatioUri, response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Book.CHECK_OUT_BOOKS_ENDPOINT)]
        public async Task<ActionResult<RegBookResponseObj>> CheckOutBook([FromBody] AddCheckOutActivityRequestObj requestObj)
        {
            CheckOutCommand command = _mapper.Map<CheckOutCommand>(requestObj);
            var response = await _mediator.Send(command);
            if (response.CheckOutActivityId < 1 || !response.Status.IsSuccessful)
            {
                return BadRequest(response);
            }
            var locatioUri = _uriService.GetBookUri(response.CheckOutActivityId.ToString());
            return Created(locatioUri, response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Book.CHECK_IN_BOOKS_ENDPOINT)]
        public async Task<ActionResult<CheckOutActivityResponseObj>> CheckInBook([FromBody] EditCheckOutActivityRequestObj requestObj)
        {
            CheckInCommand command = _mapper.Map<CheckInCommand>(requestObj);
            var response = await _mediator.Send(command);
            if (response.CheckOutActivityId < 1 || !response.Status.IsSuccessful)
            {
                return BadRequest(response);
            }
            var locatioUri = _uriService.GetBookUri(response.CheckOutActivityId.ToString());
            return Created(locatioUri, response);
        }

    }
}
