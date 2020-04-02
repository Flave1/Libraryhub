using AutoMapper;
using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands;
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
    public class CheckOutBookHandler : IRequestHandler<CheckOutCommand, CheckOutActivityResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        public CheckOutBookHandler(IBookService bookService, IMapper mapper, UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory)
        {
            _bookService = bookService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger(typeof(CheckOutBookHandler));
        }

        public async Task<CheckOutActivityResponseObj> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = await _userManager.FindByIdAsync(request.UserId);
                var checkOut = new CheckOutActivity
                {
                    UserId = request.UserId,
                    BookId = request.BookId,
                    CheckOutDate = DateTime.Today,
                    Email = currentUser.Email,
                    ExpectedReturnDate = DateTime.Today.AddDays(12),
                    FullName = currentUser.FullName,
                    NationalIdentificationNumber = currentUser.NationalIdentificationNumber??"BIMC123QWEG",
                    PhoneNumber = currentUser.PhoneNumber,
                    ReturnDate = null,
                    Status = (int)BookActivityStatus.Check_Out,
                };
                await _bookService.CheckOutBookAsync(checkOut);
                var response = _mapper.Map<CheckOutActivityResponseObj>(checkOut);
                return await Task.Run(() => response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Something went wrong", ex.InnerException.Message);
                throw new NotImplementedException("Something went wrong", ex.InnerException);
            }
        }
    }
}
