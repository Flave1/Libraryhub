using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Queries
{ 
    //public class SearchQuery : IRequest<BookResponseObj>
    //{
    //    public BookSearchObj BookSearch { get; }
    //    public SearchQuery(BookSearchObj bookSearch) { BookSearch = bookSearch; }
    //}
    public class GetAllBooksQuery : IRequest<BookResponseObj> { }

}
