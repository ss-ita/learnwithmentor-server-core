using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
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
