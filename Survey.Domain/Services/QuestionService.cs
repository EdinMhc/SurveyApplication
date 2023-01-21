namespace Survey.Domain.Services
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.Question;
    using Survey.Domain.Services.Interfaces;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class QuestionService : IQuestionService
    {
        private readonly ILogger<QuestionService> _logger;
        private IUnitOfWork _unitOfWork;

        public QuestionService(
            IUnitOfWork unitOfWork,
            ILogger<QuestionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IEnumerable<Question> GetAll(int companyId, int surveyId, string role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            PropertyValidator(companyId, surveyId);

            // User checkup with company
            var dbCompany = isAdmin
            ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? _unitOfWork.QuestionRepository.GetAll().Where(p => p.SurveyID == surveyId)
                : _unitOfWork.QuestionRepository.GetAll();
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionNotExistant);
            }

            return result;
        }

        public Question GetById(int companyId, int surveyId, int questionId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            PropertyValidator(companyId, surveyId);

            var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? _unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.QuestionID == questionId)
                : _unitOfWork.QuestionRepository.GetByID(questionId);
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionNotExistant);
            }

            return result;
        }

        public async Task<Question> CreateAsync(Question question, int companyId, int surveyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new QuestioCreateValidator(_unitOfWork, companyId, surveyId, question.AnwserBlockID).Validate(question);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                question.SurveyID = surveyId;
                // INSERT
                _unitOfWork.QuestionRepository.Add(question);
                await _unitOfWork.SaveChangesAsync();

                return question;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int surveyId, int questionId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                PropertyValidator(companyId, surveyId);

                if (questionId <= 0)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
                var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
                if (resultSurveyCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
                }

                var result = isAdmin
                    ? _unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.QuestionID == questionId)
                    : _unitOfWork.QuestionRepository.GetByID(questionId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionNotExistant);
                }

                _unitOfWork.QuestionRepository.Delete(result);
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

        public async Task<Question> UpdateAsync(Question question, int companyId, int surveyId, int questionId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new QuestionUpdateValidation(_unitOfWork, companyId, surveyId, questionId, question.AnwserBlockID).Validate(question);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbQuestion = _unitOfWork.QuestionRepository.GetAll().FirstOrDefault(x => x.QuestionID == questionId && x.SurveyID == surveyId && x.AnwserBlockID == question.AnwserBlockID)
                    ?? _unitOfWork.QuestionRepository.GetAll().FirstOrDefault(x => x.QuestionID == questionId && x.SurveyID == surveyId);
                if (dbQuestion == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionNotExistant);
                }

                dbQuestion.Code = !string.IsNullOrWhiteSpace(question.Code) ? question.Code : dbQuestion.Code;
                dbQuestion.QuestionType = !string.IsNullOrWhiteSpace(question.QuestionType) ? question.QuestionType : dbQuestion.QuestionType;
                dbQuestion.QuestionText = !string.IsNullOrWhiteSpace(question.QuestionText) ? question.QuestionText : dbQuestion.QuestionText;

                if (question.AnwserBlockID != 0 || question.AnwserBlockID! < 0)
                {
                    dbQuestion.AnwserBlockID = question.AnwserBlockID;
                }

                _unitOfWork.QuestionRepository.Update(dbQuestion);
                await _unitOfWork.SaveChangesAsync();
                return dbQuestion;
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
