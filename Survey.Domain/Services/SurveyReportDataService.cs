namespace Survey.Domain.Services
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.SurveyReportData;
    using Survey.Domain.Services.Interfaces;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class SurveyReportDataService : ISurveyReportDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public SurveyReportDataService(
            IUnitOfWork unitOfWork,
            ILogger<SurveyReportDataService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SurveyReportData> CreateAsync(SurveyReportData surveyReportData, int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new SurveyReportDataCreationValidator(_unitOfWork, companyId, surveyId, surveyReportId, surveyReportData.QuestionID, surveyReportData.AnswerID).Validate(surveyReportData);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.UserDoesNotMatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var resultSurveyReportSurvey = _unitOfWork.SurveyReportRepository.GetByID(surveyReportId);

                surveyReportData.SurveyReportID = resultSurveyReportSurvey.SurveyReportID;
                surveyReportData.CreatedDate = DateTime.Now;

                _unitOfWork.SurveyReportDataRepository.Add(surveyReportData);
                await _unitOfWork.SaveChangesAsync();

                return surveyReportData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
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
                    _logger.LogError($"Error occured: {CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero);
                }

                PropertyValidator(companyId, surveyId, surveyReportId);

                var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = isAdmin
                        ? _unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.RespondentID == respondentId && p.SurveyReportID == surveyReportId)
                        : _unitOfWork.SurveyReportDataRepository.GetByID(respondentId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataNotExistant);
                }

                _unitOfWork.SurveyReportDataRepository.Delete(result);
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

        public IEnumerable<SurveyReportData> GetAll(int companyId, int surveyId, int surveyReportId, string? role, string userId)
        {
            PropertyValidator(companyId, surveyId, surveyReportId);

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
                ? _unitOfWork.SurveyReportDataRepository.GetAll().Where(p => p.SurveyReportID == surveyReportId)
                : _unitOfWork.SurveyReportDataRepository.GetAll();

            return result;
        }

        public SurveyReportData GetById(int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                if (respondentId <= 0)
                {
                    _logger.LogError($"Error occured: {CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero);
                }

                PropertyValidator(companyId, surveyId, surveyReportId);

                var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = isAdmin
                        ? _unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.RespondentID == respondentId && p.SurveyReportID == surveyReportId)
                        : _unitOfWork.SurveyReportDataRepository.GetByID(respondentId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataNotExistant);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<SurveyReportData> UpdateAsync(SurveyReportData surveyReport, int companyId, int surveyId, int surveyReportId, int respondentId, string? role, string userId)
        {
            try
            {
                var result = new SurveyReportDataUpdateValidator(_unitOfWork, companyId, surveyId, surveyReportId, surveyReport.QuestionID, surveyReport.AnswerID, respondentId).Validate(surveyReport);
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

                var resultSurveyReportData = _unitOfWork.SurveyReportDataRepository.GetByID(surveyReportId);
                if (resultSurveyReportData == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportDataNotExistant);
                }

                resultSurveyReportData.QuestionID = surveyReport.QuestionID;
                resultSurveyReportData.AnswerID = surveyReport.AnswerID;

                _unitOfWork.SurveyReportDataRepository.Update(resultSurveyReportData);
                await _unitOfWork.SaveChangesAsync();
                return resultSurveyReportData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        private void PropertyValidator(int companyId, int surveyId, int surveyReportId)
        {

            if (surveyReportId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.SurveyReportIDBelowOrEqualToZero);
            }

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


            // COMPANY SURVEY RELATIONSHIP CHECK
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);

            if (resultSurveyCompany == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            // SURVEY SURVEYREPORT RELATIONSHIP CHECK
            var resultSurveySurveyReport = _unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.SurveyReportID == surveyReportId);
            if (resultSurveySurveyReport == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipSurveySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipSurveySurvey);
            }
        }
    }
}
