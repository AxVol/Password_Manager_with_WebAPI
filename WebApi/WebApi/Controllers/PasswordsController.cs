using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Net;
using System.Net.Mime;
using WebApi.Domain.Response.Password;
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

        /// <remarks>
        /// В хэдере аутентификации должена быть добавлена Baerer с JWT токеном, иначе упадет ошибка отсутствия авторизации
        ///  
        /// GET /Passwords/GetUserPass
        /// </remarks>
        /// <response code="200">Выдача всех принадлежащих паролей юзеру взависимости от его jwt токена</response>
        /// <response code="400">Не валидные данные</response>
        /// <response code="401">Ошибка авторизации</response>
        [Route("GetUserPass/")]
        [HttpGet]
        [ProducesResponseType(typeof(PasswordResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];

            var response = await passwordService.GetAll(token);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return new JsonResult(response.Values);
        }

        /// <remarks>
        /// В хэдере аутентификации должена быть добавлена Baerer с JWT токеном, иначе упадет ошибка отсутствия авторизации
        /// Айди при запросе можно передавать любой, запрос при обработке будет исправлен на соответствующий айди для БД
        /// id - int
        /// Остальные поля предоставляют тип - string
        ///  
        ///     POST /Passwords/CreatePass
        ///     {
        ///         "id": "0",              
        ///         "login": "string",
        ///         "password": "string",
        ///         "service": "string"
        ///     }
        /// 
        /// </remarks>
        /// <response code="401">Ошибка авторизации</response>
        /// <response code="200">Сообщение об успешно созданом пароле</response>
        /// <response code="400">Не валидные данные</response>
        [Route("CreatePass")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(PasswordViewModel model)
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];
            var response = await passwordService.Create(model, token);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return Ok();
        }

        /// <remarks>
        /// В хэдере аутентификации должена быть добавлена Baerer с JWT токеном, иначе упадет ошибка отсутствия авторизации
        /// id - int
        /// Остальные поля предоставляют тип - string
        ///  
        ///     PUT /Passwords/UpdatePass
        ///     {
        ///         "id": "0",              
        ///         "login": "string",
        ///         "password": "string",
        ///         "service": "string"
        ///     }
        /// 
        /// </remarks>
        /// <response code="401">Ошибка авторизации</response>
        /// <response code="200">Сообщение об успешно обновленном пароле</response>
        /// <response code="400">Не валидные данные</response>
        [Route("UpdatePass")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(PasswordViewModel model)
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];
            var response = await passwordService.Update(model, token);

            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return Ok();
        }

        /// <remarks>
        /// В хэдере аутентификации должена быть добавлена Baerer с JWT токеном, иначе упадет ошибка отсутствия авторизации
        /// id - int
        /// Остальные поля предоставляют тип - string
        ///  
        ///     DELETE /Passwords/DeletePass
        ///     {
        ///         "id": "0",              
        ///         "login": "string",
        ///         "password": "string",
        ///         "service": "string"
        ///     }
        /// 
        /// </remarks>
        /// <response code="401">Ошибка авторизации</response>
        /// <response code="200">Сообщение об успешно удаленном пароле</response>
        /// <response code="400">Не валидные данные</response>
        [Route("DeletePass")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(PasswordViewModel model)
        {
            string token = Request.Headers.Authorization!;
            token = token.Split(' ')[1];
            var response = await passwordService.Delete(model, token);
            
            if (response.Status == Domain.Enum.RequestStatus.Failed)
                return BadRequest(new { response.Description });

            return Ok();
        }
    }
}
