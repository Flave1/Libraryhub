using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Domain;
using Libraryhub.DomainObjs;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class OrderCommandHandler : IRequestHandler<OrderCommand, RegBookResponseObj>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderCommandHandler(ILoggerFactory loggerFactory, IMapper mapper, IOrderService orderService, IBookService bookService, UserManager<ApplicationUser> userManager)
        {
            _logger = loggerFactory.CreateLogger(typeof(OrderCommandHandler));
            _bookService = bookService;
            _userManager = userManager;
            _orderService = orderService;
            _mapper = mapper;
        }
        public async Task<RegBookResponseObj> Handle(OrderCommand request, CancellationToken cancellationToken)
        {
            try
            {

                #region Fetch List of Requested Book by a customer from Repository

                List<int> bookIDs = new List<int>();

                foreach (var requestedBookId in request.Books)
                {
                    bookIDs.Add(Convert.ToInt32(requestedBookId.BookId));
                }
                var CustomerRequestedBooks = await _bookService.GetListBooksByIDAsync(bookIDs);

                #endregion

                #region Check the avaialability of each of the  book requested by a Customer

                foreach (var requestedBookFromRepo in CustomerRequestedBooks)
                {
                    if (!requestedBookFromRepo.IsAvailable)
                    {
                        return new RegBookResponseObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = $"Currently {requestedBookFromRepo.Title} is not available"
                                }
                            }
                        };
                    }
                }

                #endregion

                #region Check if Any of the book quantity requested for exceeds the book quantity in the library

                foreach (var requestedBookFromRepo in CustomerRequestedBooks)
                {
                    var thisReqBook = request.Books.FirstOrDefault(m => m.BookId == requestedBookFromRepo.BookId);
                    if (thisReqBook.Quantity > requestedBookFromRepo.Quantity)
                    {
                        return new RegBookResponseObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = $"Quantity demanded for {requestedBookFromRepo.Title}  exceeded what we have"
                                }
                            }
                        };
                    }
                }

                #endregion

                #region Update Book repository

                var order = new Order
                {
                    CustomerId = request.CustomerId
                };
                _orderService.CreateOrder(order);
                foreach (var requestedBook in request.Books)
                {
                    var thisBookFromRepo = CustomerRequestedBooks.FirstOrDefault(m => m.BookId == requestedBook.BookId);
                    var orderItem = new OrderItem
                    {
                        BookId = requestedBook.BookId,
                        CoverPrice = thisBookFromRepo.CoverPrice,
                        Quantity = requestedBook.Quantity,
                    };
                    _orderService.CreateOrderItem(orderItem);

                    thisBookFromRepo.Quantity = thisBookFromRepo.Quantity - requestedBook.Quantity;
                    if (thisBookFromRepo.Quantity == 0) { thisBookFromRepo.IsAvailable = false; }
                    await _bookService.UpdateBookAsync(thisBookFromRepo);
                }
                await _bookService.CommitChangesAsync();
                #endregion

                return new RegBookResponseObj
                {
                    BookId = order.OrderId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
}
    }
}
