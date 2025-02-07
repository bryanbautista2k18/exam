using FluentValidation;
using api.DTOs;

namespace api.Validators 
{
    public class RequestUserDTOValidator : AbstractValidator<RequestUserDTO>
    {
        public RequestUserDTOValidator()
        {
            // Rule for Name
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(768).WithMessage("Name cannot exceed 768 characters.");

            // Rule for Email
            RuleFor(user => user.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.");

            // Rule for ConfigGenderId
            RuleFor(user => user.ConfigGenderId)
                .NotEmpty().WithMessage("Gender is required.");

            // Rule for BirthDate
            RuleFor(user => user.BirthDate)
                .NotEmpty().WithMessage("Birth Date is required.");
        }
    }
}