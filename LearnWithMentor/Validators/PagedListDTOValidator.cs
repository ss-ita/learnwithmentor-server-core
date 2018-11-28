using FluentValidation;
using LearnWithMentorDTO;

namespace LearnWithMentor.Validators
{
    public class PagedListDTOValidator<T> : AbstractValidator<PagedListDTO<T>>
    {
        public PagedListDTOValidator()
        {
            // No rules yet. Add new rules here when needed.
        }
    }
}
