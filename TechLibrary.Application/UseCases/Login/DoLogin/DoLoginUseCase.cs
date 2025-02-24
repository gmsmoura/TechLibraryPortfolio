using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.DataAccess;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Infrastructure.Security.Tokens.Access;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechLibrary.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public DoLoginUseCase(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public ResponseRegisteredUserJson Execute(RequestLoginJson requestLogin)
        {
            var dbContext = new TechLibraryDbContext();

            var user = dbContext.Users.FirstOrDefault(user => user.Email.Equals(requestLogin.Email));
            if (user is null) 
            {
                throw new InvalidLoginException();
            }

            var cryptography = new BCryptAlgorithm();
            var passwordIsValid = cryptography.Verify(requestLogin.Password, user); 

            if (passwordIsValid == false) throw new InvalidLoginException();

            return new ResponseRegisteredUserJson()
            {
                Name = user.Name,
                AccessToken = _jwtTokenGenerator.Generate(user)
            };
        }
    }
}
