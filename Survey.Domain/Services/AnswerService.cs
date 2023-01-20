namespace Survey.Domain.Services
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.Answer;
    using Survey.Domain.Services.Interfaces;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class AnswerService : IAnswerService
    {
        private readonly ILogger<AnswerService> _logger;
        private IUnitOfWork _unitOfWork;

        public AnswerService(IUnitOfWork unitOfWork,
            ILogger<AnswerService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IEnumerable<Anwser> GetAll(int companyId, int anwserBlockId, string? role, string userId)
        {
            PropertyValidator(companyId, anwserBlockId);
            bool isAdmin = role == AdminHelper.Admin;

            var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            return isAdmin
                    ? _unitOfWork.AnwserRepository.GetAll().Where(p => p.AnwserBlockID == anwserBlockId)
                    : _unitOfWork.AnwserRepository.GetAll();
        }

        public Anwser GetById(int companyId, int anwserBlockId, int anwserId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;
            PropertyValidator(companyId, anwserBlockId);

            if (anwserId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero);
            }

            var dbCompany = isAdmin
            ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : _unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = isAdmin
                   ? _unitOfWork.AnwserRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == anwserBlockId && x.AnwserID == anwserId)
                   : _unitOfWork.AnwserRepository.GetByID(anwserId);
            if (result == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserNotExistant}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);
            }

            return result;
        }

        public async Task<Anwser> CreateAsync(Anwser answer, int companyId, int anwserBlockId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerValidationCreation(_unitOfWork, companyId, anwserBlockId).Validate(answer);
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

                answer.AnwserBlockID = anwserBlockId;

                _unitOfWork.AnwserRepository.Add(answer);
                await _unitOfWork.SaveChangesAsync();

                return answer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<Anwser> UpdateAsync(Anwser answer, int companyId, int anwserBlockId, int answerId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerValidationUpdate(_unitOfWork, companyId, anwserBlockId, answerId).Validate(answer);
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

                var dbAnswer = _unitOfWork.AnwserRepository.GetByID(answerId);
                if (dbAnswer == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);
                }

                dbAnswer.AnwserText = !string.IsNullOrWhiteSpace(answer.AnwserText) ? answer.AnwserText : dbAnswer.AnwserText;

                _unitOfWork.AnwserRepository.Update(dbAnswer);
                await _unitOfWork.SaveChangesAsync();

                return dbAnswer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<bool> DeleteAsync(int companyId, int answerBlockId, int answerId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;
                PropertyValidator(companyId, answerBlockId);

                if (answerId <= 0)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = isAdmin
                       ? _unitOfWork.AnwserRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == answerBlockId && x.AnwserID == answerId)
                       : _unitOfWork.AnwserRepository.GetByID(answerId);
                if (result == null)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserNotExistant}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);
                }


                _unitOfWork.AnwserRepository.Delete(result);
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

        private void PropertyValidator(int companyId, int anwserBlockId)
        {

            if (companyId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
            }

            var anwserBlockValidation = _unitOfWork.AnwserBlockRepository.GetByID(anwserBlockId);

            if (anwserBlockValidation == null)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockNotExistant}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
            }

            if (anwserBlockValidation.CompanyID != companyId)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany);
            }

            if (anwserBlockId <= 0)
            {
                _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero);
            }
        }
    }
}
