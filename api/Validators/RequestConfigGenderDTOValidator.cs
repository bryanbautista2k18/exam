using FluentValidation;
using api.DTOs;

namespace api.Validators 
{
    public class RequestConfigGenderDTOValidator : AbstractValidator<RequestConfigGenderDTO>
    {
        public RequestConfigGenderDTOValidator()
        {
            // Rule for Title
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(256).WithMessage("Title cannot exceed 256 characters.");

            // Rule for Description
            RuleFor(x => x.Description)
                .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters.");
        }
    }
}