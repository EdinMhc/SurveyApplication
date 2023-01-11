namespace Survey.Domain.Services.QuestionService
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.Question;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class QuestionService : IQuestionService
    {
        private readonly ILogger<Survey.Domain.Services.QuestionService.QuestionService> logger;
        private IUnitOfWork unitOfWork;

        public QuestionService(
            IUnitOfWork UnitOfWork,
            ILogger<Survey.Domain.Services.QuestionService.QuestionService> logger)
        {
            this.unitOfWork = UnitOfWork;
            this.logger = logger;
        }

        public IEnumerable<Question> GetAll(int companyId, int surveyId, string role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            this.PropertyValidator(companyId, surveyId);

            // User checkup with company
            var dbCompany = isAdmin
            ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? this.unitOfWork.QuestionRepository.GetAll().Where(p => p.SurveyID == surveyId)
                : this.unitOfWork.QuestionRepository.GetAll();
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionNotExistant);
            }

            return result;
        }

        public Question GetById(int companyId, int surveyId, int questionId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            this.PropertyValidator(companyId, surveyId);

            var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? this.unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.QuestionID == questionId)
                : this.unitOfWork.QuestionRepository.GetByID(questionId);
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

                var result = new QuestioCreateValidator(this.unitOfWork, companyId, surveyId, question.AnwserBlockID).Validate(question);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                question.SurveyID = surveyId;
                // INSERT
                this.unitOfWork.QuestionRepository.Add(question);
                await this.unitOfWork.SaveChangesAsync();

                return question;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int surveyId, int questionId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                this.PropertyValidator(companyId, surveyId);

                if (questionId <= 0)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
                var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
                if (resultSurveyCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
                }

                var result = isAdmin
                    ? this.unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.QuestionID == questionId)
                    : this.unitOfWork.QuestionRepository.GetByID(questionId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionNotExistant);
                }

                this.unitOfWork.QuestionRepository.Delete(result);
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

        public async Task<Question> UpdateAsync(Question question, int companyId, int surveyId, int questionId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new QuestionUpdateValidation(this.unitOfWork, companyId, surveyId, questionId, question.AnwserBlockID).Validate(question);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbQuestion = this.unitOfWork.QuestionRepository.GetAll().FirstOrDefault(x => x.QuestionID == questionId && x.SurveyID == surveyId && x.AnwserBlockID == question.AnwserBlockID)
                    ?? this.unitOfWork.QuestionRepository.GetAll().FirstOrDefault(x => x.QuestionID == questionId && x.SurveyID == surveyId);
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

                this.unitOfWork.QuestionRepository.Update(dbQuestion);
                await this.unitOfWork.SaveChangesAsync();
                return dbQuestion;
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
