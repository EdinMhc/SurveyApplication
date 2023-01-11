namespace Survey.Domain.Services.FluentValidation.Company
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class CompanyValidator : AbstractValidator<Company>
    {

        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public CompanyValidator(IUnitOfWork unitOfWork)
        {

            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;

            this.RuleFor(x => x.CompanyName)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            this.RuleFor(x => x.Address)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            this.RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .Length(5, 255)
                .EmailAddress().WithMessage("Email is not a valid email address.");
        }

        public override ValidationResult Validate(ValidationContext<Company> context)
        {
            this.result = base.Validate(context);
            //this.ValidateUser();

            return this.result;
        }
    }
}
