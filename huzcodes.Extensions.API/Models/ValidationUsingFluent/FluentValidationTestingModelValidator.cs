using FluentValidation;

namespace huzcodes.Extensions.API.Models.ValidationUsingFluent
{
    public class FluentValidationRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
    public class FluentValidationTestingModelValidator : AbstractValidator<FluentValidationRequest>
    {
        public FluentValidationTestingModelValidator()
        {
            RuleFor(o => o.FirstName).NotEmpty();
            RuleFor(o => o.LastName).NotEmpty();
        }
    }
}
