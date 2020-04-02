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
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookId);
        }

        public async Task<Book> GetBookByISBNAsync(string isbn)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.ISBN.ToLower().Trim() == isbn.ToLower().Trim());
        }

        public async Task<IEnumerable<Book>> GetBookByStatusAsync(bool status)
        {
            return await _dataContext.Books.Where(x => x.IsAvailable == status).ToListAsync();
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.Title.ToLower().Trim() == title.ToLower().Trim());
        }
         
        public async Task<bool> UpdateBookAsync(Book book)
        {
            var bookToUpdate = await GetBookByIdAsync(book.BookId);
            _dataContext.Entry(bookToUpdate).CurrentValues.SetValues(book);

            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> CheckOutBookAsync(CheckOutActivity req)
        {
            await _dataContext.BookActivities.AddAsync(req);
            int created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> CheckInBookAsync(CheckOutActivity req)
        {
                var bookActivity = await GetCheckOutActivityById(req.CheckOutActivityId);
                _dataContext.Entry(bookActivity).CurrentValues.SetValues(req);

                var checkedIn = await _dataContext.SaveChangesAsync();
                return checkedIn > 0;
        }

        public async Task<CheckOutActivity> GetCheckOutActivityById(int id)
        {
            return await _dataContext.BookActivities.FirstOrDefaultAsync(x => x.CheckOutActivityId == id);
        }

        public async Task<bool> PenalizeAsync(BookPenalty penalty)
        {
            await _dataContext.BookPenalties.AddAsync(penalty);
            int created = await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<BookPenalty>> GetAllPenaltyChargiesAsync()
        {
            return await _dataContext.BookPenalties.ToListAsync();
        }

        public async Task<IEnumerable<BookPenalty>> GetPenaltyChargiesForCustomer(string userId)
        {
            return await _dataContext.BookPenalties.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
