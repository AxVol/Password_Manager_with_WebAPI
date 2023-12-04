using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.ViewModels.User;
using WebApi.Service;
using WebApi.Service.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJWTService jWTService;

        public UserController(IUserService service, IJWTService jWT)
        {
            userService = service;
            jWTService = jWT;
        }

        [Route("Authentication")]
        [HttpPost]
        public async Task<IActionResult> Authentication(LoginViewModel model)
        {
            var response = await userService.Login(model);

            if (response.Status == Domain.Enum.RequestStatus.Success)
            {
                string token = await jWTService.GetToken(response.Value);

                return new JsonResult(token);
            }
            else if (response.Description == "Неверный пароль")
            {
                response.Description = response.Value.Id.ToString();

                return Unauthorized(new { response.Description });
            }

            return Unauthorized(new { response.Description });
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var response = await userService.Register(model);

            if (response.Status == Domain.Enum.RequestStatus.Success)
            {
                string token = await jWTService.GetToken(response.Value);

                return new JsonResult(token);
            }

            return BadRequest(new { response.Description });
        }

        [Route("BlockAccount")]
        [HttpPost]
        public async Task<IActionResult> BlockAccount(BlockUserViewModel block)
        {
            await userService.BlockAccount(block.Id);

            return Ok();
        }
    }
}
