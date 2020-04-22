using Libraryhub.Data;
using Libraryhub.DomainObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task<bool> AddNewBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> BookExistAsync(Book book);
        Task<IEnumerable<Book>> GetListBooksByIDAsync(List<int> bookIds);


        Task<bool> CheckOutBookAsync(BooksActivity req);
        Task<bool> CheckInBookAsync(BooksActivity req);
        Task<BooksActivity> GetCheckOutActivityById(int id);
        Task<BooksActivity> GetBookActivityByBookId(int bookId);
        Task<IEnumerable<BooksActivity>> GetAllCheckoutActivities();
        Task<IEnumerable<BooksActivity>> GetCheckoutActivitiesByStatus(int status);


        Task<bool> PenalizeAsync(BookPenalty penalty);
        Task<IEnumerable<BookPenalty>> GetAllPenaltyChargiesAsync();
        Task<IEnumerable<BookPenalty>> GetPenaltyChargiesForCustomer(string customerId);

         

        DataContext Context();
        Task SaveChangesAsync();
    }
}
