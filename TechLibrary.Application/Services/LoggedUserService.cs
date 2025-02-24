using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Domain.Entities;
using TechLibrary.Infrastructure.DataAccess;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService
{
    private readonly HttpContext _httpContext;
    public LoggedUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public User GetUser(TechLibraryDbContext dbContext)
    {
        if (!_httpContext.Request.Headers.TryGetValue("Authorization", out var authentication))
        {
            throw new UnauthorizedAccessException("Authorization header is missing.");
        }

        var token = authentication.ToString()["Bearer ".Length..].Trim();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
        var userId = Guid.Parse(identifier);

        return dbContext.Users.First(user => user.Id == userId);
    }
}