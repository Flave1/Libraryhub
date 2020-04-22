using AutoMapper;
using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Domain;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Orders
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand, RegOrderResponseObj>
    {
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public ConfirmOrderHandler(IOrderService orderService, ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger(typeof(OrderHandler));
        }
        public async Task<RegOrderResponseObj> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                #region Check if this Order has a detail 
                var CustomerOrderDetailFromRepo = await _orderService.GetOrderDetailByOrderId(request.OrderId);
                if(CustomerOrderDetailFromRepo == null)
                {
                    return new RegOrderResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Something went wrong please contact admin",  
                            }
                        }
                    };
                }
                #endregion

                #region Check if the Order has been delivered

                if(CustomerOrderDetailFromRepo.OrderStatus == OrderStatus.Delivered)
                {
                    return new RegOrderResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This Order has alread being delivered",
                            }
                        }
                    };
                }

                #endregion

                #region Check if it's admin or customer requesting to confirme this order
                var user = await _userManager.FindByIdAsync(request.CustomerId);
                if(CustomerOrderDetailFromRepo.CustomerId != request.CustomerId || (CustomerOrderDetailFromRepo.CustomerId != request.CustomerId && !await _userManager.IsInRoleAsync(user, Role.Admin.ToString())))
                {
                    return new RegOrderResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Please you can't confirm some one else's order",
                            }
                        }
                    };
                }

                #endregion

                #region Confirm this order by updating order details
                CustomerOrderDetailFromRepo.OrderStatus = OrderStatus.Delivered;
                CustomerOrderDetailFromRepo.DateDelivered = DateTime.Now;

                try
                {
                    await Task.Run(async () => {
                        await _orderService.UpdateOrderDetails(CustomerOrderDetailFromRepo);
                        await _orderService.SaveChangesAsync();
                        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                    });
                }
                catch (ArgumentOutOfRangeException) {  }
               
                
                return new RegOrderResponseObj
                {
                    OrderId = CustomerOrderDetailFromRepo.OrderId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                    }
                };
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
