using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(login => login.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(login => login.Password)
                .NotEmpty();
        }
    }
}
