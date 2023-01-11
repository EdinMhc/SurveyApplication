namespace Survey.Domain.Services.SurveyService
{
    using global::FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.Survey;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class SurveyService : ISurveyService
    {
        private readonly ILogger<SurveyService> logger;
        private IUnitOfWork unitOfWork;

        public SurveyService(
            IUnitOfWork unitOfWork,
            ILogger<SurveyService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public IEnumerable<Surveys> GetAll(int companyId, string? role, string? userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            if (companyId <= 0)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
            }

            var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = role == AdminHelper.Admin
                ? this.unitOfWork.SurveysRepository.GetAll().Include("SurveyReport").Where(x => x.CompanyID == companyId)
                : this.unitOfWork.SurveysRepository.GetAll().Include("SurveyReport");

            return result;
        }

        public Surveys GetById(int surveyid, int companyId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            if (companyId <= 0)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDvalidation);
            }

            if (surveyid <= 0)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyIDValidation);
            }

            var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = role == AdminHelper.Admin
                ? this.unitOfWork.SurveysRepository.GetAll().Include("SurveyReport").FirstOrDefault(x => x.CompanyID == companyId && x.SurveyID == surveyid)
                : this.unitOfWork.SurveysRepository.GetAll().Include("SurveyReport").FirstOrDefault(x => x.SurveyID == surveyid);
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyNotExistant);
            }

            return result;
        }

        public async Task<Surveys> CreateAsync(Surveys survey, int companyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new SurveyValidator(this.unitOfWork, companyId, userId).Validate(survey);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                survey.CreateDate = DateTime.Now;
                survey.CreatedBy = dbCompany.CompanyName;
                survey.CompanyID = dbCompany.CompanyID;

                this.unitOfWork.SurveysRepository.Add(survey);
                await this.unitOfWork.SaveChangesAsync();

                return survey;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int surveyId, int companyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                if (companyId <= 0)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDvalidation);
                }

                if (surveyId <= 0)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyIDValidation);
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                // In order to cascade delete you have to include the relevant questions or tables
                var result = role == AdminHelper.Admin
                    ? this.unitOfWork.SurveysRepository.GetAll().Include(x => x.Questions).FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId)
                    : this.unitOfWork.SurveysRepository.GetAll().Include(x => x.Questions).FirstOrDefault(p => p.SurveyID == surveyId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyNotExistant);
                }

                this.unitOfWork.SurveysRepository.Delete(result);
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

        public async Task<Surveys> UpdateAsync(Surveys survey, int surveyId, int companyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                // Validation Survey Update
                var result = new SurveyUpdateValidator(this.unitOfWork, companyId, userId, surveyId).Validate(survey);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                // Checking userId and role
                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbSurvey = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
                if (dbSurvey == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyNotExistant);
                }

                dbSurvey.SurveyName = !string.IsNullOrWhiteSpace(survey.SurveyName) ? survey.SurveyName : dbSurvey.SurveyName;

                dbSurvey.IsActive = survey.IsActive;

                this.unitOfWork.SurveysRepository.Update(dbSurvey);
                await this.unitOfWork.SaveChangesAsync();

                return dbSurvey;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }
    }
}