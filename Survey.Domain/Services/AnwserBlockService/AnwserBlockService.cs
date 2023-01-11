namespace Survey.Domain.Services.AnwserBlockService
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.AnswerBlock;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class AnwserBlockService : IAnwserBlockService
    {
        private readonly ILogger<Survey.Domain.Services.AnwserBlockService.AnwserBlockService> logger;
        private IUnitOfWork unitOfWork;

        public AnwserBlockService(
            IUnitOfWork unitOfWork,
            ILogger<Survey.Domain.Services.AnwserBlockService.AnwserBlockService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        // Task For self: If superAdmin accesses the service GetAll, Get and Delete.
        // The companyId and surveyId becomes obsolete in that case. He just has to be
        // able to erase, get or getall information without entering surveyId or CompanyId

        public IEnumerable<AnwserBlock> GetAll(int companyId, int surveyId, string? role, string userId)
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

            // CHECKING RELATIONSHIP SURVEY COMPANY
            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? this.unitOfWork.AnwserBlockRepository.GetAll().Where(x => x.CompanyID == companyId)
                : this.unitOfWork.AnwserBlockRepository.GetAll();

            return result;
        }

        public AnwserBlock GetById(int anwserBlockId, int companyId, int surveyId, string role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            this.PropertyValidator(companyId, surveyId);

            if (anwserBlockId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero);
            }

            // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }


            // CHECK FOR ROLES AND GIVE RELEVANT DATA
            var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = isAdmin
                ? this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == anwserBlockId && x.CompanyID == companyId)
                : this.unitOfWork.AnwserBlockRepository.GetByID(anwserBlockId);
            if (result == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
            }

            return result;
        }

        public async Task<AnwserBlock> CreateAsync(AnwserBlock anwserBlock, int companyId, int surveyId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerBlockCreationValidation(this.unitOfWork, companyId, surveyId).Validate(anwserBlock);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                // User Check
                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                anwserBlock.CompanyID = companyId;
                this.unitOfWork.AnwserBlockRepository.Add(anwserBlock);
                await this.unitOfWork.SaveChangesAsync();

                return anwserBlock;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int surveyId, int answerBlockId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                this.PropertyValidator(companyId, surveyId);

                if (answerBlockId <= 0)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero);
                }

                // USER AND ROLE CHECK
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
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
                }

                var result = isAdmin
                    ? this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == answerBlockId && x.CompanyID == companyId)
                    : this.unitOfWork.AnwserBlockRepository.GetByID(answerBlockId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
                }

                this.unitOfWork.AnwserBlockRepository.Delete(result);
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

        public async Task<AnwserBlock> UpdateAsync(AnwserBlock anwserBlock, int companyId, int surveyId, int anwserBlockId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerBlockUpdateValidation(this.unitOfWork, companyId, surveyId, anwserBlockId).Validate(anwserBlock);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                // Checking user and role
                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbAnwserBlock = this.unitOfWork.AnwserBlockRepository.GetByID(anwserBlockId);
                if (dbAnwserBlock == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
                }

                dbAnwserBlock.AnwserBlockName = !string.IsNullOrWhiteSpace(anwserBlock.AnwserBlockName) ? anwserBlock.AnwserBlockName : dbAnwserBlock.AnwserBlockName;
                dbAnwserBlock.BlockType = !string.IsNullOrWhiteSpace(anwserBlock.BlockType) ? anwserBlock.BlockType : dbAnwserBlock.BlockType;

                if (anwserBlock.CodeOfAnwserBlock != 0 || anwserBlock.CodeOfAnwserBlock! < 0)
                {
                    dbAnwserBlock.CodeOfAnwserBlock = anwserBlock.CodeOfAnwserBlock;
                }

                this.unitOfWork.AnwserBlockRepository.Update(dbAnwserBlock);
                await this.unitOfWork.SaveChangesAsync();

                return dbAnwserBlock;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
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
