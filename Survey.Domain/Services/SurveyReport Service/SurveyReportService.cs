namespace Survey.Domain.Services.SurveyReport_Service
{
    using global::FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.SurveyReport;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class SurveyReportService : ISurveyReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;

        public SurveyReportService(
            IUnitOfWork UnitOfWork,
            ILogger<SurveyReportService> logger)
        {
            this.unitOfWork = UnitOfWork;
            this.logger = logger;
        }

        public IEnumerable<SurveyReport> GetAll(int companyId, int surveyId, string? userId, string? role)
        {
            this.PropertyValidator(companyId, surveyId);

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
                ? this.unitOfWork.SurveyReportRepository.GetAll().Include("SurveyReportData").Where(p => p.SurveyID == surveyId)
                : this.unitOfWork.SurveyReportRepository.GetAll().Include("SurveyReportData");

            return result;
        }

        public SurveyReport GetById(int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            if (surveyReportId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyReportIDValidation}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDValidation);
            }

            this.PropertyValidator(companyId, surveyId);

            // USER AND COMPANY CHECK
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
                    ? this.unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.SurveyReportID == surveyReportId)
                    : this.unitOfWork.SurveyReportRepository.GetByID(surveyReportId);
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportNotExistant);
            }

            return result;
        }

        public async Task<SurveyReport> CreateAsync(SurveyReport surveyReport, int companyId, int surveyId, string? role, string userId)
        {
            try
            {

                var result = new SurveyReportCreationValidator(this.unitOfWork, companyId, surveyId).Validate(surveyReport);
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

                var surveyCheck = this.unitOfWork.SurveysRepository.GetByID(surveyId);

                surveyReport.SurveyID = surveyCheck.SurveyID;
                surveyReport.CreateDate = surveyCheck.CreateDate;

                this.unitOfWork.SurveyReportRepository.Add(surveyReport);
                await this.unitOfWork.SaveChangesAsync();

                return surveyReport;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            try
            {
                if (surveyReportId == 0)
                {
                    this.logger.LogError($"Error occured: {CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero);
                }

                bool isAdmin = role == AdminHelper.Admin;

                if (surveyReportId <= 0)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyReportIDValidation}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDValidation);
                }

                this.PropertyValidator(companyId, surveyId);

                // USER AND COMPANY CHECK
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
                        ? this.unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.SurveyReportID == surveyReportId)
                        : this.unitOfWork.SurveyReportRepository.GetByID(surveyReportId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportNotExistant);
                }

                this.unitOfWork.SurveyReportRepository.Delete(result);
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

        public async Task<SurveyReport> UpdateAsync(SurveyReport surveyReport, int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            try
            {

                var result = new SurveyReportUpdateValidator(this.unitOfWork, companyId, surveyId, surveyReportId).Validate(surveyReport);
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

                var dbSurveyReport = this.unitOfWork.SurveyReportRepository.GetByID(surveyReportId);

                dbSurveyReport.IsCompleted = surveyReport.IsCompleted;

                this.unitOfWork.SurveyReportRepository.Update(dbSurveyReport);
                await this.unitOfWork.SaveChangesAsync();

                return dbSurveyReport;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        private void PropertyValidator(int companyId, int surveyId)
        {

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
        }
    }
}
