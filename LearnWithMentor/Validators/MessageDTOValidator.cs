using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class MessageDTOValidator : AbstractValidator<MessageDto>
    {
        public MessageDTOValidator()
        {
            RuleFor(message => message.Text)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_MESSAGE_TEXT_LENGTH)
                .WithMessage("Message is too long");
        }
    }
}
