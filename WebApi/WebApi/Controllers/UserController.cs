using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using WebApi.Domain.Entity;
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

        /// <remarks>
        /// Поле логина адаптируется к входу по самому логину или почте пользователя
        /// Все поля типа - string
        ///  
        ///     POST /User/Authentication
        ///     {             
        ///         "login": "string",
        ///         "password": "string",
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Успешная авторизация и отправка jwt токена пользователя в ответе</response>
        /// <response code="401">Ошибка авторизации</response>
        [Route("Authentication")]
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
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

        /// <remarks>
        /// Все поля типа - string
        ///  
        ///     POST /User/Register
        ///     {
        ///         "email": "string",
        ///         "login": "string",
        ///         "password": "string",
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Успешная регистрация и отправка jwt токена пользователя в ответе</response>
        /// <response code="400">Не валидные данные</response>
        [Route("Register")]
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
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

        /// <remarks>
        /// После нескольких неудачных попыток входа, клиент отправляет айди пользователя 
        /// для его блокировки из-за подозрительной активности
        /// id - long
        ///  
        ///     POST /User/BlockAccount
        ///     {             
        ///         "id": "0"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Подтверждение успешной блокировки</response>
        [Route("BlockAccount")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> BlockAccount(BlockUserViewModel block)
        {
            await userService.BlockAccount(block.Id);

            return Ok();
        }
    }
}
