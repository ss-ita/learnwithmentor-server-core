using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class PagedListDTOValidator<T> : AbstractValidator<PagedListDto<T>>
    {
        public PagedListDTOValidator()
        {
            // No rules yet. Add new rules here when needed.
        }
    }
}
