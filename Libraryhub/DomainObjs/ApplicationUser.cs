using Libraryhub.DomainObjs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Libraryhub.Domain
{

    [NotMapped]
    public class ApplicationUser : IdentityUser 
    { 
        public string FullName { get; set; } 
        public string NationalIdentificationNumber { get; set; } 

        
    }
}
