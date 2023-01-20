namespace Survey.Domain.Services.FluentValidation.SurveyReport
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportUpdateValidator : AbstractValidator<SurveyReport>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int SurveyReportId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyReportUpdateValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId)
        {
            CompanyId = companyId;
            SurveyId = surveyId;
            SurveyReportId = surveyReportId;
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
        }

        public override ValidationResult Validate(ValidationContext<SurveyReport> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();
            ValidateSurveyReport();

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

        private void ValidateSurveyReport()
        {
            if (SurveyReportId == 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurveyReport = _unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.SurveyReportID == SurveyReportId);
            if (dbSurveyReport == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportNotExistant]));
            }
        }
    }
}
