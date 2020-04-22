using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.Services
{
    public interface IAppSettingService
    {
        Task<bool> CheckoutReminder(int switchValue);

        Task<bool> LeftOverReminder(int switchValue);
    }
}
