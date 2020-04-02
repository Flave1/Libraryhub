using AutoMapper;
using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.ErrorResponses;
using Libraryhub.Contracts.RequestObjs;
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
    public class CheckInHandler : IRequestHandler<CheckInCommand, CheckOutActivityResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CheckInHandler(IBookService bookService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(CheckInHandler));
            _bookService = bookService;
            _mapper = mapper; 
        }

        public async Task<CheckOutActivityResponseObj> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentCheckedOutBook = await _bookService.GetCheckOutActivityById(request.CheckOutActivityId);
                var checkInReq = new CheckOutActivity
                {
                    UserId = currentCheckedOutBook.UserId,
                    BookId = request.BookId,
                    CheckOutDate = currentCheckedOutBook.CheckOutDate,
                    Email = currentCheckedOutBook.Email,
                    ExpectedReturnDate = currentCheckedOutBook.ExpectedReturnDate,
                    FullName = currentCheckedOutBook.FullName,
                    NationalIdentificationNumber = currentCheckedOutBook.NationalIdentificationNumber,
                    PhoneNumber = currentCheckedOutBook.PhoneNumber,
                    ReturnDate = DateTime.Today,
                    Status = (int)BookActivityStatus.Check_In,
                    CheckOutActivityId = request.CheckOutActivityId
                };

                if (request.ReturnDate > currentCheckedOutBook.ExpectedReturnDate)
                { 
                    var lateDays = (request.ReturnDate - currentCheckedOutBook.ExpectedReturnDate).TotalDays;
                    var penaltyFee = 200 * Convert.ToInt64(lateDays);
                    bool ExceededReturnDate = true;


                    if (ExceededReturnDate && request.PenaltyFee == null || request.PenaltyFee == 0)
                        return await Task.Run(() => new CheckOutActivityResponseObj { UserMessage = new UserMessage { Message = $"User expected to pay a penalty fee of {penaltyFee} for {Convert.ToInt64(lateDays)} day(s) late return" } });

                    if(ExceededReturnDate && penaltyFee != request.PenaltyFee && request.PenaltyFee != 0)
                        return await Task.Run(() => new CheckOutActivityResponseObj { UserMessage = new UserMessage { Message = $"User expected to pay a penalty fee of {penaltyFee} for {Convert.ToInt64(lateDays)} day(s) late return not {request.PenaltyFee}" } });


                    var penaltyReq = new BookPenalty()
                    {
                        ExpectedReturnDate = currentCheckedOutBook.ExpectedReturnDate,
                        NumberOfDaysLate = Convert.ToInt64(lateDays),
                        CheckOutActivityId = request.CheckOutActivityId,
                        BookId = request.BookId,
                        PenaltyFee = penaltyFee,
                        UserId = currentCheckedOutBook.UserId

                    };
                    var penalized = await _bookService.PenalizeAsync(penaltyReq);
                    if (penalized)
                    {
                        await _bookService.CheckInBookAsync(checkInReq);
                        return _mapper.Map<CheckOutActivityResponseObj>(checkInReq);
                       
                    }

                }
               await _bookService.CheckInBookAsync(checkInReq);
                return  _mapper.Map<CheckOutActivityResponseObj>(checkInReq);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Something went wrong", ex.InnerException.Message);
                throw new NotImplementedException("Something went wrong", ex.InnerException);
            }
          

        }
    }
}
