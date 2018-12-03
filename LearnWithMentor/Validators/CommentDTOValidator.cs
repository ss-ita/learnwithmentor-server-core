using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class CommentDTOValidator : AbstractValidator<CommentDTO>
    {
        public CommentDTOValidator()
        {
            RuleFor(comment => comment.Text)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_COMMENT_TEXT_LENGTH)
                .WithMessage("Comment text too long");
        }
    }
}
