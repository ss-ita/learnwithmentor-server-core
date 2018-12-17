using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class UserIdentityDTOValidator : AbstractValidator<UserIdentityDTO>
    {
        public UserIdentityDTOValidator()
        {
            RuleFor(userIdentity => userIdentity.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email not valid");
            RuleFor(userIdentity => userIdentity.LastName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_NAME_LENGTH)
                .WithMessage("LastName too long")
                .Matches(ValidationConstants.ONLY_LETTERS_AND_NUMBERS)
                .WithMessage("LastName not valid");
            RuleFor(userIdentity => userIdentity.FirstName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_NAME_LENGTH)
                .WithMessage("FirstName too long")
                .Matches(ValidationConstants.ONLY_LETTERS_AND_NUMBERS)
                .WithMessage("FirstName not valid");
            RuleFor(userIdentity => userIdentity.Password)
                .NotEmpty();
        }
    }
}
