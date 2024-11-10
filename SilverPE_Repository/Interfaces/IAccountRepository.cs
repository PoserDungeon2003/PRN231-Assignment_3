using SilverPE_BOs.Models;
using SilverPE_Repository.Request;
using SilverPE_Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository.Interfaces
{
    public interface IAccountRepository
    {
        public Task<AccountLoginResponse> GetBranchAccount(string email, string password);
    }
}
