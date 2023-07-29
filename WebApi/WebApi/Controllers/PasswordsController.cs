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
            PasswordViewModel model = new PasswordViewModel()
            {
                SecretToken = secretToken
            };

            var response = await passwordService.GetAll(model);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
            {
                return new JsonResult(response.Description);
            }

            return new JsonResult(response.Value);
        }

        [Route("CreatePass")]
        [HttpPost]
        public async Task<IActionResult> Create(PasswordViewModel model)
        {
            var response = await passwordService.Create(model);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
            {
                return new JsonResult(response.Description);
            }

            return new JsonResult(response.Value);
        }

        [Route("UpdatePass")]
        [HttpPut]
        public async Task<IActionResult> Update(PasswordViewModel model)
        {
            var response = await passwordService.Update(model);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
            {
                return new JsonResult(response.Description);
            }

            return new JsonResult(response.Value);
        }

        [Route("DeletePass")]
        [HttpDelete]
        public async Task<IActionResult> Delete(PasswordViewModel model)
        {
            var response = await passwordService.Delete(model);

            return new JsonResult(response.Description);
        }
    }
}
