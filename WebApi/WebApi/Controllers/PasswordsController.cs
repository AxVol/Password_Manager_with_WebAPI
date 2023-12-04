using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.ViewModels.Password;
using WebApi.Service.Interfaces;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordService passwordService;

        public PasswordsController(IPasswordService service) 
        {
            passwordService = service;
        }

        [Route("GetUserPass/")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];

            var response = await passwordService.GetAll(token);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return new JsonResult(response.Values);
        }

        [Route("CreatePass")]
        [HttpPost]
        public async Task<IActionResult> Create(PasswordViewModel model)
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];
            var response = await passwordService.Create(model, token);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return new JsonResult(response.Value);
        }

        [Route("UpdatePass")]
        [HttpPut]
        public async Task<IActionResult> Update(PasswordViewModel model)
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];
            var response = await passwordService.Update(model, token);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return new JsonResult(response.Value);
        }

        [Route("DeletePass")]
        [HttpDelete]
        public async Task<IActionResult> Delete(PasswordViewModel model)
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];
            var response = await passwordService.Delete(model, token);

            return new JsonResult(response.Description);
        }
    }
}
