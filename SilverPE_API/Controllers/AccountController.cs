using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SilverPE_Repository.Interfaces;
using SilverPE_Repository.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SilverPE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest loginRequest)
        {
            var response = await _accountRepository.GetBranchAccount(loginRequest.Email, loginRequest.Password);
            if (response == null)
            {
                return BadRequest(new
                {
                    Message = "Invalid username or password",
                });
            }
            return Ok(response);
        }
    }
}
