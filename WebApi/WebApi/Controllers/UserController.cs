using Microsoft.AspNetCore.Http;
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
            throw new NotImplementedException();
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        { 
            var response = await userService.Register(model);

            if (response.Status == Domain.Enum.RequestStatus.Success)
            {
                return new JsonResult(response.Value);
            }
            
            return new JsonResult(response.Description);
        }

        [Route("UpdateToken")]
        [HttpPut]
        public async Task<IActionResult> Update(string secretToken)
        {
            throw new NotImplementedException();
        }
    }
}
