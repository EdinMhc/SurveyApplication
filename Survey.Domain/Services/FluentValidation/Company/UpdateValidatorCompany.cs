namespace Survey.Domain.Services.FluentValidation.Company
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class UpdateValidatorCompany : AbstractValidator<Company>
    {
        private readonly int CompanyId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public UpdateValidatorCompany(IUnitOfWork unitOfWork, int companyId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;

            RuleFor(x => x.CompanyName)
                .Must(x => ValidateCompanyName(x)).WithMessage("Company Name insufficient characters");

            RuleFor(x => x.Address)
                .Length(2, 255)
                .Unless(x => x.Address == null || x.Address == string.Empty);

            RuleFor(x => x.Email)
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
            _result = base.Validate(context);
            ValidateCompany();

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }
    }
}
