using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class PlanDTOValidator : AbstractValidator<PlanDto>
    {
        public PlanDTOValidator()
        {
            RuleFor(plan => plan.Name)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_PLAN_NAME_LENGTH)
                .WithMessage("Plan name too long");
            RuleFor(plan => plan.Description)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAX_PLAN_DESCRIPTION_LENGTH)
                .WithMessage("Plan description too long");
            RuleFor(plan => plan.Published)
                .NotEmpty();
        }
    }
}
