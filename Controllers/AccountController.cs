using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rest.Models;
using Rest.Repository;

namespace Rest.Controllers
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

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {
                return Ok();
            }
            
            return Unauthorized();
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _accountRepository.GetUsersAsync();
            return Ok(result);
        }
    }
}
