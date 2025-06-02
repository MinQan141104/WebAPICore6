using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApiNetCore6.Models;
using MyApiNetCore6.Repositories;

namespace MyApiNetCore6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        public AccountController(IAccountRepository repo)
        {
            accountRepository = repo;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await accountRepository.SignUpAsync(model);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return StatusCode(500);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await accountRepository.SignInAsync(model);

            if(string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
