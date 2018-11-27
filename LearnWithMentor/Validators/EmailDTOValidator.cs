using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class EmailDTOValidator : AbstractValidator<EmailDTO>
    {
        public EmailDTOValidator()
        {
            RuleFor(email => email.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
