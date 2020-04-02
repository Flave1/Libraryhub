using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Queries
{
    public class GetAllBooksQuery : IRequest<List<BookResponseObj>> { }

    public class GetBookByTitleQuery : IRequest<BookResponseObj>
    {
        public string Title { get; }
        public GetBookByTitleQuery(string title) { Title = title; }
    }
    public class GetBookByISBNQuery : IRequest<BookResponseObj>
    {
        public string ISBN { get; }
        public GetBookByISBNQuery(string isbn) { ISBN = isbn; }
    }
    public class GetBookByStatusQuery : IRequest<List<BookResponseObj>>
    {
        public bool Status { get; }
        public GetBookByStatusQuery(bool status) { Status = status; }
    }

}
