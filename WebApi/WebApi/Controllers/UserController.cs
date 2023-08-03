using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Domain.ViewModels.User;
using WebApi.Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService service) 
        {
            userService = service;
        }

        [Route("Authentication")]
        [HttpPost]
        public async Task<IActionResult> Authentication(LoginViewModel model) 
        {
            var response = await userService.Login(model);

            if (response.Status == Domain.Enum.RequestStatus.Success)
                return new JsonResult(response.Value); 

            return NotFound(new { response.Description });
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        { 
            var response = await userService.Register(model);

            if (response.Status == Domain.Enum.RequestStatus.Success)
                return new JsonResult(response.Value);
            
            return BadRequest(new { response.Description });
        }

        [Route("UpdateToken")]
        [HttpPut]
        public async Task<IActionResult> Update(string secretToken)
        {
            var response = await userService.UpdateToken(secretToken);

            return Ok(new { response.Value });
        }
    }
}
