using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Survey.Domain.Services.FluentValidation.SurveyReport;
using Survey.Domain.Services.Interfaces;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Repositories;

namespace Survey.Domain.Services
{

    public class SurveyReportService : ISurveyReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public SurveyReportService(
            IUnitOfWork UnitOfWork,
            ILogger<SurveyReportService> logger)
        {
            _unitOfWork = UnitOfWork;
            _logger = logger;
        }

        public IEnumerable<SurveyReport> GetAll(int companyId, int surveyId, string? userId, string? role)
        {
            PropertyValidator(companyId, surveyId);

            bool isAdmin = role == AdminHelper.Admin;

            var dbCompany = isAdmin
            ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // COMPANY SURVEY RELATIONSHIP CHECK
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? _unitOfWork.SurveyReportRepository.GetAll().Include("SurveyReportData").Where(p => p.SurveyID == surveyId)
                : _unitOfWork.SurveyReportRepository.GetAll().Include("SurveyReportData");

            return result;
        }

        public SurveyReport GetById(int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            if (surveyReportId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyReportIDValidation}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDValidation);
            }

            PropertyValidator(companyId, surveyId);

            // USER AND COMPANY CHECK
            var dbCompany = isAdmin
            ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // COMPANY SURVEY RELATIONSHIP CHECK
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                    ? _unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.SurveyReportID == surveyReportId)
                    : _unitOfWork.SurveyReportRepository.GetByID(surveyReportId);
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

                var result = new SurveyReportCreationValidator(_unitOfWork, companyId, surveyId).Validate(surveyReport);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }


                var dbCompany = role == AdminHelper.Admin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.UserDoesNotMatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var surveyCheck = _unitOfWork.SurveysRepository.GetByID(surveyId);

                surveyReport.SurveyID = surveyCheck.SurveyID;
                surveyReport.CreateDate = surveyCheck.CreateDate;

                _unitOfWork.SurveyReportRepository.Add(surveyReport);
                await _unitOfWork.SaveChangesAsync();

                return surveyReport;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
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
                    _logger.LogError($"Error occured: {CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero);
                }

                bool isAdmin = role == AdminHelper.Admin;

                if (surveyReportId <= 0)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyReportIDValidation}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDValidation);
                }

                PropertyValidator(companyId, surveyId);

                // USER AND COMPANY CHECK
                var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                // COMPANY SURVEY RELATIONSHIP CHECK
                var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
                if (resultSurveyCompany == null)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
                }

                var result = isAdmin
                        ? _unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.SurveyReportID == surveyReportId)
                        : _unitOfWork.SurveyReportRepository.GetByID(surveyReportId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportNotExistant);
                }

                _unitOfWork.SurveyReportRepository.Delete(result);
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

        public async Task<SurveyReport> UpdateAsync(SurveyReport surveyReport, int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            try
            {

                var result = new SurveyReportUpdateValidator(_unitOfWork, companyId, surveyId, surveyReportId).Validate(surveyReport);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }


                var dbCompany = role == AdminHelper.Admin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.UserDoesNotMatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbSurveyReport = _unitOfWork.SurveyReportRepository.GetByID(surveyReportId);

                dbSurveyReport.IsCompleted = surveyReport.IsCompleted;

                _unitOfWork.SurveyReportRepository.Update(dbSurveyReport);
                await _unitOfWork.SaveChangesAsync();

                return dbSurveyReport;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        private void PropertyValidator(int companyId, int surveyId)
        {

            if (companyId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
            }

            if (surveyId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero);
            }
        }
    }
}
