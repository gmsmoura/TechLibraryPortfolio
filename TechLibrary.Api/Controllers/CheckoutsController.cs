using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Api.UseCases.Checkouts;

namespace TechLibrary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CheckoutsController : ControllerBase
{
    private readonly RegisterBookCheckoutUseCase _useCase;

    public CheckoutsController(RegisterBookCheckoutUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost]
    [Route("{bookId}")]
    public IActionResult BookCheckout(Guid bookId)
    {
        _useCase.Execute(bookId);

        return Ok("The loan was realized");
    }
}