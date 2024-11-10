using SilverPE_BOs.Models;
using SilverPE_DAO;
using SilverPE_Repository.Interfaces;
using SilverPE_Repository.Request;
using SilverPE_Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ITokenRepository _tokenRepository;
        public AccountRepository(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public async Task<AccountLoginResponse> GetBranchAccount(string email, string password)
        {
            var branchAccount = await AccountDAO.Instance.GetBranchAccount(email, password);

            if (branchAccount != null)
            {
                return new AccountLoginResponse
                {
                    Email = branchAccount.EmailAddress,
                    FullName = branchAccount.FullName,
                    Id = branchAccount.AccountId,
                    Role = branchAccount.Role,
                    Token = _tokenRepository.GenerateToken(branchAccount)
                };
            }

            return null;
        }
    }
}
