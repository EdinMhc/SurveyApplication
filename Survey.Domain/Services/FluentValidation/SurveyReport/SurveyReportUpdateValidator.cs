namespace Survey.Domain.Services.FluentValidation.SurveyReport
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportUpdateValidator : AbstractValidator<SurveyReport>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int surveyReportId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public SurveyReportUpdateValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId)
        {
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.surveyReportId = surveyReportId;
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
        }

        public override ValidationResult Validate(ValidationContext<SurveyReport> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
            this.ValidateSurveyReport();

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
            }

            var companyCheck = this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == this.companyId);
            if (companyCheck == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
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
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurveyReport = this.unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.SurveyReportID == this.surveyReportId);
            if (dbSurveyReport == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportNotExistant]));
            }
        }
    }
}
