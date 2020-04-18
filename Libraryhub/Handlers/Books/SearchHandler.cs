using AutoMapper;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.DomainObjs;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class SearchHandler : IRequestHandler<BookSearchObj, BookResponseObj>
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        public SearchHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        public async Task<BookResponseObj> Handle(BookSearchObj request, CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllBooksAsync(); 

            List<Book> bookList = new List<Book>();
            List<Book> searched = new List<Book>();

            if (!string.IsNullOrEmpty(request.AccessionNo))
            {
                searched = books.Where(x => x.AccessionNo == request.AccessionNo).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.Author))
            {
                searched = books.Where(x => x.Author == request.Author).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.ClassificationNo))
            {
                searched = books.Where(x => x.ClassificationNo == request.ClassificationNo).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.Color))
            {
                searched = books.Where(x => x.Color == request.Color).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.ISBN))
            {
                searched = books.Where(x => x.ISBN == request.ISBN).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.Language))
            {
                searched = books.Where(x => x.Language == request.Language).ToList();
                bookList.AddRange(searched);
            }
            else if (!string.IsNullOrEmpty(request.Section))
            {
                searched = books.Where(x => x.Section == request.Section).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.Size))
            {
                searched = books.Where(x => x.Size == request.Size).ToList();
                bookList.AddRange(searched);
            }
            if(request.Status)
            {
                bookList = books.Where(x => x.IsAvailable == request.Status).ToList();
                bookList.AddRange(bookList);
            }
            if (!string.IsNullOrEmpty(request.Title))
            {
                searched = books.Where(x => x.Title == request.Title).ToList();
                bookList.AddRange(searched);
            }
            if (!string.IsNullOrEmpty(request.PublishYear))
            {
                searched = books.Where(x => x.PublishYear == request.PublishYear).ToList();
                bookList.AddRange(searched);
            }
            else { }

            var response = new BookResponseObj
            {
                Books = _mapper.Map<List<BookObj>>(bookList),
                Status = new APIResponseStatus
                {
                    IsSuccessful = bookList == null ? true : false,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = bookList == null ? "Search Complete!! No Record Found" : null
                    }
                }
            };
            return response;
        }
    }
}
