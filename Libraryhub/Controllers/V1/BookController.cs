using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.V1;
using Libraryhub.CustomError;
using Libraryhub.Data;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Libraryhub.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ILogger _logger;
        public BookController(IMediator mediator, IBookService bookService, IMapper mapper, IUriService uriService, ILoggerFactory loggerFactory)
        {
            
            _uriService = uriService;
            _mediator = mediator; 
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger(typeof(BookController));
        }

        [HttpGet(ApiRoutes.Book.LOAD_ALL_BOOKS_ENDPOINT)]
        public async Task<ActionResult<IList<BookResponseObj>>> GetAllPostAsync()
        {
            var query = new GetAllBooksQuery();
            var result = await _mediator.Send(query);
            _logger.LogInformation("Unable to Process request Flave123");
            return Ok(result);
             
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Book.CREATE_BOOK_ENDPOINT)]
        public async Task<IActionResult> Create([FromBody] AddBookRequestObj requestObj)
        {
            try
            {
                AddBookCommand command = _mapper.Map<AddBookCommand>(requestObj);
                var result = await _mediator.Send(command);
                var locatioUri = _uriService.GetBookUri(result.BookId.ToString());
                return Created(locatioUri, _mapper.Map<BookResponseObj>(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
        }

        [HttpPost(ApiRoutes.Book.LOAD_BOOK_BY_TITLE_ENDPOINT)]
        public async Task<IActionResult> GetBookByTitle([FromBody] BookByTitleSearchObj req)
        {
            try
            {
                var query = new GetBookByTitleQuery(req.Title);
                var result = await _mediator.Send(query);
                return result != null ? (IActionResult)Ok(result) : NotFound(new NotFoundError("Book not available"));
            }
            catch (Exception ex)   
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
         
        }

        [HttpPost(ApiRoutes.Book.LOAD_BOOK_BY_ISBN_ENDPOINT)]
        public async Task<IActionResult> GetBookByISBN([FromBody] BookByISBNSearchObj req)
        {
            try
            {
                var query = new GetBookByISBNQuery(req.ISBN);
                var result = await _mediator.Send(query);
                return result != null ? (IActionResult)Ok(result) : NotFound(new NotFoundError("Book not available"));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
        }

        [HttpPost(ApiRoutes.Book.LOAD_BOOK_BY_STATUS_ENDPOINT)]
        public async Task<ActionResult<IEnumerable<BookResponseObj>>> GetBookByStatus([FromBody] BookByStatusSearchObj req)
        {
            try
            {
                var query = new GetBookByStatusQuery(req.Status);
                var result = await _mediator.Send(query);
                return result != null ? (ActionResult)Ok(result) : NotFound(new NotFoundError("No available Book"));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
        }

        [HttpPost(ApiRoutes.Book.CHECK_OUT_BOOKS_ENDPOINT)]
        public async Task<IActionResult> CheckOutBook([FromBody] AddCheckOutActivityRequestObj requestObj)
        {
            try
            { 
                CheckOutCommand command = _mapper.Map<CheckOutCommand>(requestObj);
                var result = await _mediator.Send(command);
                var locatioUri = _uriService.GetBookUri(result.ToString());
                return Created(locatioUri, result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
        }

        [HttpPost(ApiRoutes.Book.CHECK_IN_BOOKS_ENDPOINT)]
        public async Task<IActionResult> CheckInBook([FromBody] EditCheckOutActivityRequestObj requestObj)
        {
            try
            {
                CheckInCommand command = _mapper.Map<CheckInCommand>(requestObj);
                var result = await _mediator.Send(command);
                if(result.UserMessage != null) return BadRequest(new BadRequestError(result.UserMessage.Message)); 
                var locatioUri = _uriService.GetBookUri(result.ToString());
                return Created(locatioUri, result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
        }

        [HttpPost(ApiRoutes.Book.GET_ALL_BOOK_PEANALTY_CHARGIES_ENDPOINT)]
        public async Task<ActionResult<IList<PenaltyChargeResponseObj>>> GetAllBookPenaltyChargies()
        {
            var query = new GetAllPenaltyChargeQuery();
            var result = await _mediator.Send(query);
            return Ok(result);

        }

        [HttpPost(ApiRoutes.Book.GET_CUSTOMER_PENALTY_CHARGIES_ENDPOINT)]
        public async Task<IActionResult> GetCustomerPenaltyChargies([FromBody] CustomerPenaltyChargeSearchObj req)
        {
            try
            {
                var query = new GetBookByISBNQuery(req.UserId);
                var result = await _mediator.Send(query);
                return result != null ? (IActionResult)Ok(result) : NotFound(new NotFoundError("Search Complete! No Matching record"));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unable to Process request", ex.InnerException.Message);
                return BadRequest(new BadRequestError("Unable to Process request", ex.InnerException.Message));
            }
        }
    }
}
