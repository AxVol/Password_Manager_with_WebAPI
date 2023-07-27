using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.ViewModels.User;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController() { }

        [Route("Authentication")]
        [HttpPost]
        public async Task<IActionResult> Auth(LoginViewModel model) 
        {
            throw new NotImplementedException();
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            throw new NotImplementedException();
        }

        [Route("UpdateToken")]
        [HttpPut]
        public async Task<IActionResult> Update(string secretToken)
        {
            throw new NotImplementedException();
        }
    }
}
