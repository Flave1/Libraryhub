using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.Services
{
     public interface IUriService
    {
        Uri GetBookUri(string postId);
    }
}
