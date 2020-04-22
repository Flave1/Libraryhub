using Libraryhub.Service.Services;
using Libraryhub.AppEnum;
using System.Threading.Tasks;
using Libraryhub.Service.BackgroundTask;
using System;
using Libraryhub.MailHandler;
using System.Collections.Generic;
using Libraryhub.Data;
using Microsoft.Extensions.DependencyInjection; 
using Libraryhub.DomainObjs;
using Libraryhub.MailHandler.Service; 
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Libraryhub.Domain;
using System.Net.Sockets;

namespace Libraryhub.Service.ServiceImplementation
{
    public class AppSettingService : IAppSettingService
    { 
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private IEnumerable<BooksActivity> awaitingActivities;
        private readonly IEmailConfiguration _emailConfiguration;
        public AppSettingService(IBackgroundTaskQueue queue, ILoggerFactory loggerFactory,
            IServiceScopeFactory serviceScopeFactory, IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            Queue = queue;
            _emailService = emailService;
            _logger = loggerFactory.CreateLogger(typeof(AppSettingService));
            _serviceScopeFactory = serviceScopeFactory;
            _emailConfiguration = emailConfiguration;
        }
        public IBackgroundTaskQueue Queue { get; }

        public async Task<bool> LeftOverReminder(int switchValue)
        {
            if (switchValue == (int)Switch.ON)
            {
                Queue.QueueBackgroundWorkItem(async token =>
                {
                    using(var scope = _serviceScopeFactory.CreateScope())
                    { 
                        try
                        {
                            var scopedServices = scope.ServiceProvider;
                            var dataContext = scopedServices.GetRequiredService<DataContext>();
                            var BooksRemaining2 = await dataContext.Books.Where(x => x.Quantity <= 2).ToListAsync();
                            foreach(var book in BooksRemaining2)
                            {
                               SendLeftOverMail(book);
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred trying to send  " + "Email. Error: {Message}", ex.Message);
                        }
                        await Task.Delay(TimeSpan.FromMilliseconds(100), token);
                    }
                });
            }
            else
            { 
            }
            return await Task.Run(() => true);
        }

        public async Task<bool> CheckoutReminder(int switchValue)
        {
            if (switchValue == (int)Switch.ON)
            {
                Queue.QueueBackgroundWorkItem(async token =>
                {
                    var guid = Guid.NewGuid().ToString();

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var dataContext = scopedServices.GetRequiredService<DataContext>();
                        var ReturnDate = 0;
                        try
                        {
                            awaitingActivities = await dataContext.BookActivities.Where(x => x.Status == (int)BookActivityStatus.Check_Out).ToListAsync();
                            
                            var users = await dataContext.Users.ToListAsync();
                           
                            foreach (var awaitingActivity in awaitingActivities)
                            {
                                var customer = users.FirstOrDefault(c => c.Id == awaitingActivity.CustomerId);
                                ReturnDate = Convert.ToInt32((awaitingActivity.ExpectedReturnDate - awaitingActivity.CheckOutDate).TotalDays);
                                var bookToBeReturned = await dataContext.Books.FindAsync(awaitingActivity.BookId);
                                if(awaitingActivity.ExpectedReturnDate.Date == DateTime.Today)
                                {
                                    SendCheckinReminder(awaitingActivity, bookToBeReturned, customer);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred trying to send  " + "Email. Error: {Message}", ex.Message);
                        }

                        await Task.Delay(TimeSpan.FromMinutes(ReturnDate), token);

                    }

                    _logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
                });
            }
            return await Task.Run(() => true);
        }


        private async void SendCheckinReminder(BooksActivity awaitingActivity, Book bookToBeReturned, ApplicationUser customer)
        {
            try
            {
                await _emailService.Send(new EmailMessage
                {
                    Content = $"Dear {awaitingActivity.FullName}, <br> This is to remind" +
                $" you, the book with " +
                $"Title : {bookToBeReturned.Title}  you collected " +
                $"on the {awaitingActivity.CheckOutDate} or {(awaitingActivity.CheckOutDate - DateTime.Today).TotalDays} " +
                $"day(s) ago is due for return.",
                    Subject = "Library Reminder",
                    FromAddresses = new List<EmailAddress>
                 {
                   new EmailAddress{ Address = _emailConfiguration.SmtpUsername, Name = "Library Hub" }
                  },
                    ToAddresses = new List<EmailAddress>
                  {
                   new EmailAddress{ Address = customer.Email, Name = customer.FullName }
                   },

                });
            }
            catch (SocketException) {  }
            
        }

        private async void SendLeftOverMail(Book book)
        {
            try
            {
                await _emailService.Send(new EmailMessage
                {
                    Subject = "Library Reminder",
                    Content = $"Dear Libraryhub admin, <br> This is to remind you, " +
                $"the book with <b>Title</b> : {book.Title}  <b>ISBN</b> : {book.ISBN}  is just remaining : {book.Quantity} ",
                    FromAddresses = new List<EmailAddress>
                 {
                  new EmailAddress{ Address = _emailConfiguration.SmtpUsername, Name = "Library Hub" }
                  },
                    ToAddresses = new List<EmailAddress>
                  {
                   new EmailAddress{ Address = _emailConfiguration.SmtpUsername, Name = "Library Hub" }
                   },
                });
            }
            catch (SocketException) {  }
        }

    }
}
