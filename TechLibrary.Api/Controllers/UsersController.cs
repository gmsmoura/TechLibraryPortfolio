using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RegisterUserUseCase _registerUserUseCase;

        public UsersController(RegisterUserUseCase registerUserUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register(RequestUserJson requestUser)
        {
            var response = _registerUserUseCase.Execute(requestUser);
            return Created(string.Empty, response);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Created();
        }
    }
}
