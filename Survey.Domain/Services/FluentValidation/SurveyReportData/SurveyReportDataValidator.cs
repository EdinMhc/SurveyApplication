namespace Survey.Domain.Services.FluentValidation.SurveyReportData
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportDataValidator : AbstractValidator<SurveyReportData>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int surveyReportId;
        private readonly int respondentId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public SurveyReportDataValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId, int respondentId)
        {
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.surveyReportId = surveyReportId;
            this.respondentId = respondentId;
            this.unitOfWork = unitOfWork;
            this.result = new ValidationResult();
        }

        public override ValidationResult Validate(ValidationContext<SurveyReportData> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
            this.ValidateSurveyReport();
            this.ValidateSurveyReportData();

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
            }
        }

        private void ValidateSurvey()
        {
            if (this.surveyId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.CompanyID == this.companyId);
            if (resultSurveyCompany == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipCompanySurvey]));
            }
        }

        private void ValidateSurveyReport()
        {
            if (this.surveyReportId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurveyReport = this.unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.SurveyReportID == this.surveyReportId);
            if (dbSurveyReport == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportNotExistant]));
            }

            var resultSurveyReportCompany = this.unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.SurveyReportID == this.surveyReportId);
            if (resultSurveyReportCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipSurveySurvey);
            }
        }

        private void ValidateSurveyReportData()
        {
            if (this.respondentId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyReportDataId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero]));
            }
        }
    }
}
