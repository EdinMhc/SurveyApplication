namespace Survey.Domain.Services.FluentValidation.Company
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class UpdateValidatorCompany : AbstractValidator<Company>
    {
        private readonly int companyId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public UpdateValidatorCompany(IUnitOfWork unitOfWork, int companyId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;

            this.RuleFor(x => x.CompanyName)
                .Must(x => this.ValidateCompanyName(x)).WithMessage("Company Name insufficient characters");

            this.RuleFor(x => x.Address)
                .Length(2, 255)
                .Unless(x => x.Address == null || x.Address == string.Empty);

            this.RuleFor(x => x.Email)
                .Length(5, 255)
                .EmailAddress().WithMessage("Email is not a valid email address.")
                .Unless(x => x.Email == null || x.Email == string.Empty);
        }

        private bool ValidateCompanyName(string companyName)
        {
            if (string.IsNullOrEmpty(companyName)) return true;

            if (companyName.Length > 2 && companyName.Length < 255) return true;

            return false;
        }

        public override ValidationResult Validate(ValidationContext<Company> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }
    }
}
