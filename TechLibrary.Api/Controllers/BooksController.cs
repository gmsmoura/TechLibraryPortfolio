using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Books.Filter;
using TechLibrary.Application.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly FilterBookUseCase _filterBookUseCase;

        public BooksController(FilterBookUseCase filterBookUseCase)
        {
            _filterBookUseCase = filterBookUseCase;
        }

        [HttpGet("Filter")]
        [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Filter(int pageNumber, string? title)
        {
            var request = new RequestFilterBooksJson
            {
                PageNumber = pageNumber,
                Title = title
            };

            var result = _filterBookUseCase.Execute(request);

            if (result.Books.Count > 0)
                return Ok(result);

            return NoContent();
        }
    }
}
