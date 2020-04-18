using Libraryhub.Contracts.V1;
using Libraryhub.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.ServiceImplementation
{
    public class UriService : IUriService
    {
        private readonly string _baseUri; 
        public UriService(string baseUri)
        {
            _baseUri = baseUri; 
        }


        public Uri GetBookUri(string bookId)
        {
            return new Uri(_baseUri + ApiRoutes.Book.LOAD_SINGLE_BOOK_BY_ID_ENDPOINT.Replace("{bookId}", bookId));
        }

        public Uri GetUri()
        {
            return new Uri(_baseUri);
        }
    }
}
