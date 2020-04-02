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
        Task<Book> GetBookByTitleAsync(string title);
        Task<Book> GetBookByISBNAsync(string isbn);
        Task<IEnumerable<Book>> GetBookByStatusAsync(bool status);
        Task<bool> AddNewBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);

        Task<bool> CheckOutBookAsync(CheckOutActivity req);
        Task<bool> CheckInBookAsync(CheckOutActivity req);
        Task<CheckOutActivity> GetCheckOutActivityById(int id);
        Task<bool> PenalizeAsync(BookPenalty penalty);
        Task<IEnumerable<BookPenalty>> GetAllPenaltyChargiesAsync();
        Task<IEnumerable<BookPenalty>> GetPenaltyChargiesForCustomer(string userId);
    }
}
