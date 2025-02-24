using FluentValidation.Results;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Domain.Entities;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.DataAccess;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Infrastructure.Security.Tokens.Access;

namespace TechLibrary.Application.UseCases.Users.Register
{
    public class RegisterUserUseCase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public RegisterUserUseCase(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public ResponseRegisteredUserJson Execute(RequestUserJson requestUser)
        {
            var dbContext = new TechLibraryDbContext();
            Validate(requestUser, dbContext);

            var cryptography = new BCryptAlgorithm();
            var entity = new User
            {
                Email = requestUser.Email,
                Name = requestUser.Name,
                Password = cryptography.HashPassword(requestUser.Password),
            };

            dbContext.Users.Add(entity);
            dbContext.SaveChanges();

            return new ResponseRegisteredUserJson
            {
                Name = entity.Name,
                AccessToken = _jwtTokenGenerator.Generate(entity)
            };
        }

        private void Validate(RequestUserJson requestUser, TechLibraryDbContext dbContext) 
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(requestUser);
            var existUserWithEmail = dbContext.Users.Any(user => user.Email.Equals(requestUser.Email));

            if (existUserWithEmail)
                result.Errors.Add(new ValidationFailure("Email", "Email already exist in the system"));

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
