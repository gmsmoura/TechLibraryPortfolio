using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Login.DoLogin;
using TechLibrary.Application.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DoLoginUseCase _doLoginUseCase;

        public LoginController(DoLoginUseCase doLoginUseCase)
        {
            _doLoginUseCase = doLoginUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
        public IActionResult DoLogin(RequestLoginJson requestLogin)
        {
            var response = _doLoginUseCase.Execute(requestLogin);

            return Ok(response);
        }
    }
}
