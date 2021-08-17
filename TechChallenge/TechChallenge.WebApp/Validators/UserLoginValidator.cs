using FluentValidation;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.WebApp.Validators
{
    public class UserLoginValidator : AbstractValidator<User>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}