using Libraryhub.Data;
using Libraryhub.DomainObjs;
using Libraryhub.Service.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Repository.Cache
{
    public class CachedBookService //: IBookService
    {
        private readonly IBookService _bookService;
        private readonly ConcurrentDictionary<int, Book> _cache = new ConcurrentDictionary<int, Book>();
        private readonly ConcurrentDictionary<Guid, List<Book>> _cachedList = new ConcurrentDictionary<Guid, List<Book>>();
        public CachedBookService(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            if (_cache.ContainsKey(bookId))
            {
                return  _cache[bookId];
            }
            var book = await _bookService.GetBookByIdAsync(bookId);
            _cache.TryAdd(bookId, book);
            return book;
        }
        public Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> AddNewBookAsync(Book book)
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> BookExistAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckInBookAsync(BooksActivity req)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckOutBookAsync(BooksActivity req)
        {
            throw new NotImplementedException();
        }

        public DataContext Context()
        {
            throw new NotImplementedException();
        }
         
        public Task<IEnumerable<BooksActivity>> GetAllCheckoutActivities()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookPenalty>> GetAllPenaltyChargiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BooksActivity> GetBookActivityByBookId(int bookId)
        {
            throw new NotImplementedException();
        }
         
       

        public Task<IEnumerable<BooksActivity>> GetCheckoutActivitiesByStatus(int status)
        {
            throw new NotImplementedException();
        }

        public Task<BooksActivity> GetCheckOutActivityById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetListBooksByIDAsync(List<int> bookIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookPenalty>> GetPenaltyChargiesForCustomer(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PenalizeAsync(BookPenalty penalty)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
