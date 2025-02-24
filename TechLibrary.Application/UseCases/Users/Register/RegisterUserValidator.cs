using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Application.UseCases.Users.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestUserJson>
    {
        public RegisterUserValidator() 
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("The name cannot be empty");
            RuleFor(request => request.Email).NotEmpty().EmailAddress().WithMessage("The email cannot be empty or invalid");
            RuleFor(request => request.Password).NotEmpty().WithMessage("The password is required");

            When(request => string.IsNullOrEmpty(request.Password) == false, () =>
            {
                RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6).WithMessage("The password must be six characters or longer");
            });
        }
    }
}
