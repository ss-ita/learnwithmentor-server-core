using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class GroupDTOValidator : AbstractValidator<GroupDto>
    {
        public GroupDTOValidator()
        {
            RuleFor(group => group.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_GROUP_NAME_LENGTH)
                .WithMessage("Group name too long");
        }
    }
}
