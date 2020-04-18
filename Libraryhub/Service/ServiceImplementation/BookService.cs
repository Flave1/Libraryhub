using Libraryhub.Data;
using Libraryhub.DomainObjs;
using Libraryhub.Service.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.ServiceImplementation
{
    public class BookService : IBookService
    {
        private DataContext _dataContext;
        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddNewBookAsync(Book book)
        { 
                await _dataContext.Books.AddAsync(book);
                var created = await _dataContext.SaveChangesAsync();
                return created > 0; 
        }
         
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();
            var result = await queryable.Include(x => x.CheckOutActivities).ToListAsync();
            return result;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            var result = await _dataContext.Books.Include(x => x.CheckOutActivities).SingleOrDefaultAsync(x => x.BookId == bookId);
            return result;
        }


        public async Task<bool> UpdateBookAsync(Book book)
        {
            var bookToUpdate = await GetBookByIdAsync(book.BookId);
             _dataContext.Entry(bookToUpdate).CurrentValues.SetValues(book);
            return 1 > 0;
        }

        public async Task<bool> CheckOutBookAsync(BooksActivity req)
        {
            await _dataContext.BookActivities.AddAsync(req);

            return 1 > 0;
        }

        public async Task<bool> CheckInBookAsync(BooksActivity req)
        {
            var bookActivity = await GetCheckOutActivityById(req.CheckOutActivityId);
            _dataContext.Entry(bookActivity).CurrentValues.SetValues(req);

            var checkedIn = await _dataContext.SaveChangesAsync();
            return checkedIn > 0;
        }

        public async Task<BooksActivity> GetCheckOutActivityById(int id)
        {
            return await _dataContext.BookActivities.FirstOrDefaultAsync(x => x.CheckOutActivityId == id);
        }

        public async Task<bool> PenalizeAsync(BookPenalty penalty)
        {
            await _dataContext.BookPenalties.AddAsync(penalty);
            return 1 > 0;
        }

        public async Task<IEnumerable<BookPenalty>> GetAllPenaltyChargiesAsync()
        {
            return await _dataContext.BookPenalties.ToListAsync();
        }

        public async Task<IEnumerable<BookPenalty>> GetPenaltyChargiesForCustomer(string customerId)
        {
            return await _dataContext.BookPenalties.Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task<bool> BookExistAsync(Book book)
        {
            var boolValue = await _dataContext.Books
                .AnyAsync(x => x.ISBN.ToLower().Trim() == book.ISBN.ToLower().Trim()
                || x.PublishYear == book.PublishYear
                && x.Title.ToLower().Trim() == book.Title.ToLower().Trim());

            return boolValue;
        }

        public async Task CommitChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BooksActivity>> GetAllCheckoutActivities()
        {
            return await _dataContext.BookActivities.ToListAsync();
        }

        public async Task<IEnumerable<BooksActivity>> GetCheckoutActivitiesByStatus(int status)
        {
            return await _dataContext.BookActivities.Where(x => x.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetListBooksByIDAsync(List<int> bookIds)
        {
            var requestedBooks = new List<Book>();
            foreach(var bookId in bookIds)
            {
                var books = await GetBookByIdAsync(bookId);
                requestedBooks.Add(books);
            }
            return requestedBooks;
        }

    }
}
    