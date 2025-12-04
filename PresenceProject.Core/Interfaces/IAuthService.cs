using PresenceProject.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(RegisterModel model);
        Task<string?> LoginAsync(LoginModel model);
    }
}
