namespace Survey.Domain.Services.FluentValidation.Survey
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyUpdateValidator : AbstractValidator<Surveys>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly string userId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public SurveyUpdateValidator(IUnitOfWork unitOfWork, int companyId, string userId, int surveyId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.userId = userId;

            this.RuleFor(x => x.SurveyName)
               .Length(2, 255)
               .Unless(x => x.SurveyName == null || x.SurveyName == string.Empty);
        }

        public override ValidationResult Validate(ValidationContext<Surveys> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();

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

        private void ValidateSurvey()
        {
            // VALIDATION OF COMPANY UPDATING IT'S OWN SURVEY
            var dbSurvey = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.CompanyID == this.companyId);
            if (dbSurvey == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyNotExistant]));
            }
        }
    }
}