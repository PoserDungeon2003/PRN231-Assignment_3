using Grpc.Core;
using SilverPE_gRPC.Protos;
using SilverPE_Repository.Interfaces;

namespace SilverPE_gRPC.Services
{
    public class AccountService : Account.AccountBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async override Task<AccountLoginResponse> Login(AccountLoginRequest request, ServerCallContext context)
        {
            var account = await _accountRepository.GetBranchAccount(request.Email, request.Password);
            if (account == null)
            {
                return new AccountLoginResponse
                {
                    Message = "Invalid username or password",
                    Success = false
                };
            }

            return new AccountLoginResponse
            {
                Token = account.Token,
                Id = account.Id,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role ?? 4,
                Success = true,
                Message = "Login successful"
            };
        }
    }
}
