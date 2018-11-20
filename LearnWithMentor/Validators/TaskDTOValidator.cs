using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class TaskDTOValidator : AbstractValidator<TaskDto>
    {
        public TaskDTOValidator()
        {
            RuleFor(task => task.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_TASK_NAME_LENGTH)
                .WithMessage("Task name too long");
            RuleFor(task => task.Description)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_TASK_DESCRIPTION_LENGTH)
                .WithMessage("Description too long");
            RuleFor(task => task.Private)
                .NotNull();
            RuleFor(task => task.CreatorId)
                .NotNull();
        }
    }
}
