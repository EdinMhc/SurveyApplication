namespace Survey.Domain.Services.FluentValidation.Survey
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyUpdateValidator : AbstractValidator<Surveys>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly string UserId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyUpdateValidator(IUnitOfWork unitOfWork, int companyId, string userId, int surveyId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            SurveyId = surveyId;
            UserId = userId;

            RuleFor(x => x.SurveyName)
               .Length(2, 255)
               .Unless(x => x.SurveyName == null || x.SurveyName == string.Empty);
        }

        public override ValidationResult Validate(ValidationContext<Surveys> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();

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

        private void ValidateSurvey()
        {
            // VALIDATION OF COMPANY UPDATING IT'S OWN SURVEY
            var dbSurvey = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.CompanyID == CompanyId);
            if (dbSurvey == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyNotExistant]));
            }
        }
    }
}