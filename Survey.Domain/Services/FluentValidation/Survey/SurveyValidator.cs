namespace Survey.Domain.Services.FluentValidation.Survey
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyValidator : AbstractValidator<Surveys>
    {
        private readonly int companyId;
        private readonly string userId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public SurveyValidator(IUnitOfWork unitOfWork, int companyId, string userId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;
            this.userId = userId;


            this.RuleFor(x => x.SurveyName)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            this.RuleFor(x => x.IsActive)
                .NotNull();
        }

        public override ValidationResult Validate(ValidationContext<Surveys> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            // Company Existance
            var companyInfo = this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == this.companyId && x.UserID == this.userId);
            if (companyInfo == null)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }
    }
}
