using Microsoft.EntityFrameworkCore;
using SilverPE_BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_DAO
{
    public class AccountDAO
    {
        private SilverJewelry2023DbContext _context;
        private static AccountDAO _instance;

        public static AccountDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AccountDAO();
                }
                return _instance;
            }

        }

        public AccountDAO()
        {
            _context = new SilverJewelry2023DbContext();
        }

        public async Task<BranchAccount> GetBranchAccount(string email, string password)
            => await _context.BranchAccounts.FirstOrDefaultAsync(ba => ba.EmailAddress == email && ba.AccountPassword == password);
    }
}
