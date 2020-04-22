using AutoMapper;
using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Data;
using Libraryhub.Domain;
using Libraryhub.DomainObjs;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Orders
{
    public class OrderHandler : IRequestHandler<OrderCommand, RegOrderResponseObj>
    {
        private readonly ILogger _logger;
        private readonly IBookService _bookService;
        private readonly IOrderService _orderService;
        private readonly DataContext _dataContext;
        public OrderHandler(ILoggerFactory loggerFactory, IOrderService orderService, IBookService bookService, DataContext dataContext)
        {
            _logger = loggerFactory.CreateLogger(typeof(OrderHandler));
            _bookService = bookService;
            _orderService = orderService;
            _dataContext = dataContext;
        }
        public async Task<RegOrderResponseObj> Handle(OrderCommand request, CancellationToken cancellationToken)
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
                    if (requestedBookFromRepo == null)
                    {
                        return new RegOrderResponseObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = $"Book Not available in the Library"
                                }
                            }
                        };
                    }
                }

                foreach (var requestedBookFromRepo in CustomerRequestedBooks)
                {
                    if (!requestedBookFromRepo.IsAvailable)
                    {
                        return new RegOrderResponseObj
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
                    var thisRequestedBook = request.Books.FirstOrDefault(m => m.BookId == requestedBookFromRepo.BookId);
                    if (thisRequestedBook.Quantity > requestedBookFromRepo.Quantity)
                    {
                        return new RegOrderResponseObj
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

                #region Add Order, OrderItem and OrderDetails then Update  Book repository
                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var order = new Order
                        {
                            CustomerId = request.CustomerId,
                            GrandPrice = CustomerRequestedBooks.Sum(x => x.CoverPrice) * request.Books.Sum(x => x.Quantity),
                            TotalQuantity = request.Books.Sum(x => x.Quantity)
                        };
                        _orderService.CreateOrder(order);
                        await _bookService.SaveChangesAsync();
                        foreach (var CurrentlyRequestedBook in request.Books)
                        {
                            var thisBookFromRepo = CustomerRequestedBooks.FirstOrDefault(m => m.BookId == CurrentlyRequestedBook.BookId);
                            var orderItem = new OrderItem
                            {
                                BookId = CurrentlyRequestedBook.BookId,
                                CoverPrice = thisBookFromRepo.CoverPrice,
                                Quantity = CurrentlyRequestedBook.Quantity,
                                OrderId = order.OrderId
                            };
                            await _orderService.CreateOrderItemAsync(orderItem);
                            await _bookService.SaveChangesAsync();

                            thisBookFromRepo.Quantity = (thisBookFromRepo.Quantity - CurrentlyRequestedBook.Quantity);
                            thisBookFromRepo.QuantitySold = CurrentlyRequestedBook.Quantity - thisBookFromRepo.Quantity;
                            if (thisBookFromRepo.Quantity == 0) { thisBookFromRepo.IsAvailable = false; }
                            await _bookService.UpdateBookAsync(thisBookFromRepo);
                            await _bookService.SaveChangesAsync();
                        }

                        var orderDetail = new OrderDetail
                        {
                            CustomerId = request.CustomerId,
                            DateDelivered = request.DeliveredImmediately ? DateTime.Now : (DateTime?)null,
                            DateOrdered = DateTime.Now,
                            OrderId = order.OrderId,
                            OrderStatus = request.DeliveredImmediately ? OrderStatus.Delivered : OrderStatus.Not_Delivered,
                        };
                        await _orderService.CreateOrderDetails(orderDetail);
                        await _bookService.SaveChangesAsync();

                        await _transaction.CommitAsync();

                        return new RegOrderResponseObj
                        {
                            OrderId = order.OrderId,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = true,
                            }
                        };
                    }
                    catch (SqlException ex)
                    {
                        await _transaction.RollbackAsync();
                        #region Log error with ErroId and return error response
                        var errorId = ErrorID.Generate(4);
                        _logger.LogInformation($"OrderCommandHandler{errorId}", $"Error Message{ ex?.InnerException?.Message ?? ex?.Message}");
                        return new RegOrderResponseObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Unable to process request",
                                    MessageId = $"OrderCommandHandler{errorId}",
                                    TechnicalMessage = ex?.InnerException?.Message ?? ex?.Message
                                }
                            }
                        };
                        #endregion
                    }
                    finally
                    {
                        await _transaction.DisposeAsync();
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                #region Log eroor with ErroId and return error response
                var errorId = ErrorID.Generate(4);
                _logger.LogInformation($"OrderCommandHandler{errorId}", $"Error Message{ ex?.InnerException?.Message ?? ex?.Message}");
                return new RegOrderResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Unable to process request",
                            MessageId = $"OrderCommandHandler{errorId}",
                            TechnicalMessage = ex?.InnerException?.Message ?? ex?.Message
                        }
                    }
                };
                #endregion
            }
        }
    }
}
