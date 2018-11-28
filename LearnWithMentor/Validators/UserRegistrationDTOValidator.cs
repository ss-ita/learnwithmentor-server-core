using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class UserRegistrationDTOValidator : AbstractValidator<UserRegistrationDTO>
    {
        public UserRegistrationDTOValidator()
        {
            RuleFor(userRegistration => userRegistration.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email not valid");
            RuleFor(userRegistration => userRegistration.LastName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_NAME_LENGTH)
                .WithMessage("LastName too long")
                .Matches(ValidationConstants.ONLY_LETTERS_AND_NUMBERS)
                .WithMessage("LastName not valid");
            RuleFor(userRegistration => userRegistration.FirstName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_NAME_LENGTH)
                .WithMessage("FirstName too long")
                .Matches(ValidationConstants.ONLY_LETTERS_AND_NUMBERS)
                .WithMessage("FirstName not valid");
            RuleFor(userRegistration => userRegistration.Password)
                .NotEmpty();
        }
    }
}
