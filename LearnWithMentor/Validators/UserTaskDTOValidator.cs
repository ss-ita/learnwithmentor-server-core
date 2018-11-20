using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class UserTaskDTOValidator : AbstractValidator<UserTaskDto>
    {
        public UserTaskDTOValidator()
        {
            RuleFor(userTask => userTask.PlanTaskId)
                .NotNull();
            RuleFor(userTask => userTask.State)
                .NotEmpty()
                .Matches(ValidationConstants.USERTASK_STATE)
                .WithMessage("State could be only ['P', 'D', 'A', 'R'] letters");
            RuleFor(userTask => userTask.Result)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_USERTASK_RESULT_LENGTH)
                .WithMessage("Result too long");
        }
    }
}
