using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.ViewModels.Password;
using WebApi.Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordService passwordService;

        public PasswordsController(IPasswordService service) 
        {
            passwordService = service;
        }

        [Route("GetUserPass/{SecretToken}")]
        [HttpGet]
        public async Task<IActionResult> GetAll(string secretToken)
        {
            throw new NotImplementedException();
        }

        [Route("CreatePass")]
        [HttpPost]
        public async Task<IActionResult> Create(PasswordViewModel model)
        {
            await passwordService.Create(model);
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
