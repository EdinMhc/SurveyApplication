namespace Survey.Domain.Services.FluentValidation.Company
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class CompanyValidator : AbstractValidator<Company>
    {

        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public CompanyValidator(IUnitOfWork unitOfWork)
        {

            _result = new ValidationResult();
            _unitOfWork = unitOfWork;

            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            RuleFor(x => x.Address)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .Length(5, 255)
                .EmailAddress().WithMessage("Email is not a valid email address.");
        }

        public override ValidationResult Validate(ValidationContext<Company> context)
        {
            _result = base.Validate(context);

            return _result;
        }
    }
}
