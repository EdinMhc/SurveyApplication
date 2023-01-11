namespace Survey.Domain.Services.SurveyReportDataService
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.SurveyReportData;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class SurveyReportDataService : ISurveyReportDataService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;

        public SurveyReportDataService(
            IUnitOfWork UnitOfWork,
            ILogger<SurveyReportDataService> logger)
        {
            this.unitOfWork = UnitOfWork;
            this.logger = logger;
        }

        public async Task<SurveyReportData> CreateAsync(SurveyReportData surveyReportData, int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new SurveyReportDataCreationValidator(this.unitOfWork, companyId, surveyId, surveyReportId, surveyReportData.QuestionID, surveyReportData.AnswerID).Validate(surveyReportData);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.UserDoesNotMatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var resultSurveyReportSurvey = this.unitOfWork.SurveyReportRepository.GetByID(surveyReportId);

                surveyReportData.SurveyReportID = resultSurveyReportSurvey.SurveyReportID;
                surveyReportData.CreatedDate = DateTime.Now;

                this.unitOfWork.SurveyReportDataRepository.Add(surveyReportData);
                await this.unitOfWork.SaveChangesAsync();

                return surveyReportData;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                if (respondentId <= 0)
                {
                    this.logger.LogError($"Error occured: {CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero);
                }

                this.PropertyValidator(companyId, surveyId, surveyReportId);

                var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = isAdmin
                        ? this.unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.RespondentID == respondentId && p.SurveyReportID == surveyReportId)
                        : this.unitOfWork.SurveyReportDataRepository.GetByID(respondentId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataNotExistant);
                }

                this.unitOfWork.SurveyReportDataRepository.Delete(result);
                await this.unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occurred: {ex.Message}");
                if (ex is CustomException.CustomException) throw ex;

                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public IEnumerable<SurveyReportData> GetAll(int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            this.PropertyValidator(companyId, surveyId, surveyReportId);

            bool isAdmin = role == AdminHelper.Admin;

            var dbCompany = isAdmin
            ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // COMPANY SURVEY RELATIONSHIP CHECK
            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? this.unitOfWork.SurveyReportDataRepository.GetAll().Where(p => p.SurveyReportID == surveyReportId)
                : this.unitOfWork.SurveyReportDataRepository.GetAll();

            return result;
        }

        public SurveyReportData GetById(int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                if (respondentId <= 0)
                {
                    this.logger.LogError($"Error occured: {CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero);
                }

                this.PropertyValidator(companyId, surveyId, surveyReportId);

                var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = isAdmin
                        ? this.unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.RespondentID == respondentId && p.SurveyReportID == surveyReportId)
                        : this.unitOfWork.SurveyReportDataRepository.GetByID(respondentId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataNotExistant);
                }

                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occurred: {ex.Message}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<SurveyReportData> UpdateAsync(SurveyReportData surveyReport, int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId)
        {
            try
            {
                var result = new SurveyReportDataUpdateValidator(this.unitOfWork, companyId, surveyId, surveyReportId, surveyReport.QuestionID, surveyReport.AnswerID, respondentId).Validate(surveyReport);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = role == AdminHelper.Admin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.UserDoesNotMatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var resultSurveyReportData = this.unitOfWork.SurveyReportDataRepository.GetByID(surveyReportId);
                if (resultSurveyReportData == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataNotExistant);
                }

                resultSurveyReportData.QuestionID = surveyReport.QuestionID;
                resultSurveyReportData.AnswerID = surveyReport.AnswerID;

                this.unitOfWork.SurveyReportDataRepository.Update(resultSurveyReportData);
                await this.unitOfWork.SaveChangesAsync();
                return resultSurveyReportData;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error occurred: {ex.Message}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        private void PropertyValidator(int companyId, int surveyId, int surveyReportId)
        {

            if (surveyReportId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero);
            }

            if (companyId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
            }

            if (surveyId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero);
            }


            // COMPANY SURVEY RELATIONSHIP CHECK
            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);

            if (resultSurveyCompany == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            // SURVEY SURVEYREPORT RELATIONSHIP CHECK
            var resultSurveySurveyReport = this.unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.SurveyReportID == surveyReportId);
            if (resultSurveySurveyReport == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipSurveySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipSurveySurvey);
            }
        }
    }
}
