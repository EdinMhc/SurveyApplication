using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Survey.Domain.Services.FluentValidation.Survey;
using Survey.Domain.Services.Helper_Admin;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Repositories;

namespace Survey.Domain.Services.SurveyService
{

    public class SurveyService : ISurveyService
    {
        private readonly ILogger<SurveyService> _logger;
        private IUnitOfWork _unitOfWork;

        public SurveyService(
            IUnitOfWork unitOfWork,
            ILogger<SurveyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Gets all surveys
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public IEnumerable<Surveys> GetAll(int companyId, string? role, string? userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            if (companyId <= 0)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
            }

            var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = role == AdminHelper.Admin
                ? _unitOfWork.SurveysRepository.GetAll().Include("SurveyReport").Where(x => x.CompanyID == companyId)
                : _unitOfWork.SurveysRepository.GetAll().Include("SurveyReport");

            return result;
        }

        /// <summary>
        /// Gets a specific survey
        /// </summary>
        /// <param name="surveyid"></param>
        /// <param name="companyId"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
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
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = role == AdminHelper.Admin
                ? _unitOfWork.SurveysRepository.GetAll().Include("SurveyReport").FirstOrDefault(x => x.CompanyID == companyId && x.SurveyID == surveyid)
                : _unitOfWork.SurveysRepository.GetAll().Include("SurveyReport").FirstOrDefault(x => x.SurveyID == surveyid);
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyNotExistant);
            }

            return result;
        }

        /// <summary>
        /// Creates a survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="companyId"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public async Task<Surveys> CreateAsync(Surveys survey, int companyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new SurveyValidator(_unitOfWork, companyId, userId).Validate(survey);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                survey.CreateDate = DateTime.Now;
                survey.CreatedBy = dbCompany.CompanyName;
                survey.CompanyID = dbCompany.CompanyID;

                _unitOfWork.SurveysRepository.Add(survey);
                await _unitOfWork.SaveChangesAsync();

                return survey;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        /// <summary>
        /// Delete specific survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="companyId"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
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
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = role == AdminHelper.Admin
                    ? _unitOfWork.SurveysRepository.GetAll().Include(x => x.Questions).FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId)
                    : _unitOfWork.SurveysRepository.GetAll().Include(x => x.Questions).FirstOrDefault(p => p.SurveyID == surveyId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyNotExistant);
                }

                _unitOfWork.SurveysRepository.Delete(result);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        /// <summary>
        /// Updates survey info
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="surveyId"></param>
        /// <param name="companyId"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public async Task<Surveys> UpdateAsync(Surveys survey, int surveyId, int companyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new SurveyUpdateValidator(_unitOfWork, companyId, userId, surveyId).Validate(survey);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbSurvey = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
                if (dbSurvey == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyNotExistant);
                }

                dbSurvey.SurveyName = !string.IsNullOrWhiteSpace(survey.SurveyName) ? survey.SurveyName : dbSurvey.SurveyName;

                dbSurvey.IsActive = survey.IsActive;

                _unitOfWork.SurveysRepository.Update(dbSurvey);
                await _unitOfWork.SaveChangesAsync();

                return dbSurvey;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }
    }
}