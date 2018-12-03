using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class UserWithImageDTOValidator : AbstractValidator<UserWithImageDTO>
    {
        public UserWithImageDTOValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email not valid");
            RuleFor(user => user.LastName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_NAME_LENGTH)
                .WithMessage("LastName too long")
                .Matches(ValidationConstants.ONLY_LETTERS_AND_NUMBERS)
                .WithMessage("LastName not valid");
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_NAME_LENGTH)
                .WithMessage("FirstName too long")
                .Matches(ValidationConstants.ONLY_LETTERS_AND_NUMBERS)
                .WithMessage("FirstName not valid");
        }
    }
}
