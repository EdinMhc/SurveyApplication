namespace Survey.Domain.Services.FluentValidation.SurveyReport
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportCreationValidator : AbstractValidator<SurveyReport>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyReportCreationValidator(IUnitOfWork unitOfWork, int companyId, int surveyId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            SurveyId = surveyId;

            RuleFor(x => x.IsCompleted)
                .NotNull()
                .NotEmpty();
        }

        public override ValidationResult Validate(ValidationContext<SurveyReport> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId == 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
            }

            var companyCheck = _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == CompanyId);
            if (companyCheck == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateSurvey()
        {
            if (SurveyId == 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.CompanyID == CompanyId);
            if (resultSurveyCompany == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipCompanySurvey]));

            }
        }

    }
}
