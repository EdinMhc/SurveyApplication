namespace Survey.Domain.Services.FluentValidation.SurveyReportData
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportDataValidator : AbstractValidator<SurveyReportData>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int SurveyReportId;
        private readonly int RespondentId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyReportDataValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId, int respondentId)
        {
            CompanyId = companyId;
            SurveyId = surveyId;
            SurveyReportId = surveyReportId;
            RespondentId = respondentId;
            _unitOfWork = unitOfWork;
            _result = new ValidationResult();
        }

        public override ValidationResult Validate(ValidationContext<SurveyReportData> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();
            ValidateSurveyReport();
            ValidateSurveyReportData();

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId == 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
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
                _result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurveyReport = _unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.SurveyReportID == SurveyReportId);
            if (dbSurveyReport == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportNotExistant]));
            }

            var resultSurveyReportCompany = _unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.SurveyReportID == SurveyReportId);
            if (resultSurveyReportCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipSurveySurvey);
            }
        }

        private void ValidateSurveyReportData()
        {
            if (RespondentId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyReportDataId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero]));
            }
        }
    }
}
