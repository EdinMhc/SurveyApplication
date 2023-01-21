using FluentValidation;
using Microsoft.Extensions.Logging;
using Survey.Domain.Services.FluentValidation.AnswerBlock;
using Survey.Domain.Services.Interfaces;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Repositories;

namespace Survey.Domain.Services
{

    public class AnwserBlockService : IAnwserBlockService
    {
        private readonly ILogger<AnwserBlockService> _logger;
        private IUnitOfWork _unitOfWork;

        public AnwserBlockService(
            IUnitOfWork unitOfWork,
            ILogger<AnwserBlockService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // Task For self: If superAdmin accesses the service GetAll, Get and Delete.
        // The companyId and surveyId becomes obsolete in that case. He just has to be
        // able to erase, get or getall information without entering surveyId or CompanyId

        public IEnumerable<AnwserBlock> GetAll(int companyId, int surveyId, string? role, string userId)
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

            // CHECKING RELATIONSHIP SURVEY COMPANY
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }

            var result = isAdmin
                ? _unitOfWork.AnwserBlockRepository.GetAll().Where(x => x.CompanyID == companyId)
                : _unitOfWork.AnwserBlockRepository.GetAll();

            return result;
        }

        public AnwserBlock GetById(int anwserBlockId, int companyId, int surveyId, string role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;

            PropertyValidator(companyId, surveyId);

            if (anwserBlockId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero);
            }

            // CHECKING THE RELATIONSHIP BETWEEN GIVEN COMPANY AND SURVEY
            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == surveyId && p.CompanyID == companyId);
            if (resultSurveyCompany == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }


            // CHECK FOR ROLES AND GIVE RELEVANT DATA
            var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = isAdmin
                ? _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == anwserBlockId && x.CompanyID == companyId)
                : _unitOfWork.AnwserBlockRepository.GetByID(anwserBlockId);
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

                var result = new AnswerBlockCreationValidation(_unitOfWork, companyId, surveyId).Validate(anwserBlock);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                // User Check
                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                anwserBlock.CompanyID = companyId;
                _unitOfWork.AnwserBlockRepository.Add(anwserBlock);
                await _unitOfWork.SaveChangesAsync();

                return anwserBlock;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
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

                PropertyValidator(companyId, surveyId);

                if (answerBlockId <= 0)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero);
                }

                // USER AND ROLE CHECK
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
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationshipCompanySurvey}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
                }

                var result = isAdmin
                    ? _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == answerBlockId && x.CompanyID == companyId)
                    : _unitOfWork.AnwserBlockRepository.GetByID(answerBlockId);
                if (result == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
                }

                _unitOfWork.AnwserBlockRepository.Delete(result);
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

        public async Task<AnwserBlock> UpdateAsync(AnwserBlock anwserBlock, int companyId, int surveyId, int anwserBlockId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerBlockUpdateValidation(_unitOfWork, companyId, surveyId, anwserBlockId).Validate(anwserBlock);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                // Checking user and role
                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                var dbAnwserBlock = _unitOfWork.AnwserBlockRepository.GetByID(anwserBlockId);
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

                _unitOfWork.AnwserBlockRepository.Update(dbAnwserBlock);
                await _unitOfWork.SaveChangesAsync();

                return dbAnwserBlock;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
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
