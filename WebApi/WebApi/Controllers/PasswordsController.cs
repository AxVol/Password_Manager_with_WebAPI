using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.ViewModels.Password;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        public PasswordsController() { }

        [Route("GetUserPass/{SecretToken}")]
        [HttpGet]
        public async Task<IActionResult> Get(string secretToken)
        {
            throw new NotImplementedException();
        }

        [Route("CreatePass")]
        [HttpPost]
        public async Task<IActionResult> Create(PasswordViewModel model)
        {
            throw new NotImplementedException();
        }

        [Route("UpdatePass")]
        [HttpPut]
        public async Task<IActionResult> Update(PasswordViewModel model)
        {
            throw new NotImplementedException();
        }

        [Route("DeletePass")]
        [HttpDelete]
        public async Task<IActionResult> Delete(PasswordViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
