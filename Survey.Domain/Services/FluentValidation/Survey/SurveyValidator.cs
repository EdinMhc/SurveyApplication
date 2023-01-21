namespace Survey.Domain.Services.FluentValidation.Survey
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyValidator : AbstractValidator<Surveys>
    {
        private readonly int CompanyId;
        private readonly string UserId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyValidator(IUnitOfWork unitOfWork, int companyId, string userId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            UserId = userId;


            RuleFor(x => x.SurveyName)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            RuleFor(x => x.IsActive)
                .NotNull();
        }

        public override ValidationResult Validate(ValidationContext<Surveys> context)
        {
            _result = base.Validate(context);
            ValidateCompany();

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            // Company Existance
            var companyInfo = _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == CompanyId && x.UserID == UserId);
            if (companyInfo == null)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }
    }
}
