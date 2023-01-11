namespace Survey.Domain.Services.AnwserService
{
    using global::FluentValidation;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.Answer;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class AnswerService : IAnswerService
    {
        private readonly ILogger<AnswerService> logger;
        private IUnitOfWork unitOfWork;

        public AnswerService(IUnitOfWork unitOfWork,
            ILogger<AnswerService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public IEnumerable<Anwser> GetAll(int companyId, int anwserBlockId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;
            this.PropertyValidator(companyId, anwserBlockId);

            var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = isAdmin
                    ? this.unitOfWork.AnwserRepository.GetAll().Where(p => p.AnwserBlockID == anwserBlockId)
                    : this.unitOfWork.AnwserRepository.GetAll();

            return result;
        }

        public Anwser GetById(int companyId, int anwserBlockId, int anwserId, string? role, string userId)
        {
            bool isAdmin = role == AdminHelper.Admin;
            this.PropertyValidator(companyId, anwserBlockId);

            if (anwserId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero);
            }

            var dbCompany = isAdmin
            ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
            : this.unitOfWork.CompanyRepository.GetByID(companyId);
            if (dbCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            var result = isAdmin
                   ? this.unitOfWork.AnwserRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == anwserBlockId && x.AnwserID == anwserId)
                   : this.unitOfWork.AnwserRepository.GetByID(anwserId);
            if (result == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserNotExistant}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);
            }


            return result;
        }

        public async Task<Anwser> CreateAsync(Anwser answer, int companyId, int anwserBlockId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerValidationCreation(this.unitOfWork, companyId, anwserBlockId).Validate(answer);
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

                answer.AnwserBlockID = anwserBlockId;

                this.unitOfWork.AnwserRepository.Add(answer);
                await this.unitOfWork.SaveChangesAsync();

                return answer;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<Anwser> UpdateAsync(Anwser answer, int companyId, int anwserBlockId, int answerId, string? role, string userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new AnswerValidationUpdate(this.unitOfWork, companyId, anwserBlockId, answerId).Validate(answer);
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

                var dbAnswer = this.unitOfWork.AnwserRepository.GetByID(answerId);
                if (dbAnswer == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);
                }

                dbAnswer.AnwserText = !string.IsNullOrWhiteSpace(answer.AnwserText) ? answer.AnwserText : dbAnswer.AnwserText;

                this.unitOfWork.AnwserRepository.Update(dbAnswer);
                await this.unitOfWork.SaveChangesAsync();

                return dbAnswer;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
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
                this.PropertyValidator(companyId, answerBlockId);

                if (answerId <= 0)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                var result = isAdmin
                       ? this.unitOfWork.AnwserRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == answerBlockId && x.AnwserID == answerId)
                       : this.unitOfWork.AnwserRepository.GetByID(answerId);
                if (result == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserNotExistant}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);
                }


                this.unitOfWork.AnwserRepository.Delete(result);
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

        private void PropertyValidator(int companyId, int anwserBlockId)
        {

            if (companyId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
            }

            var anwserBlockValidation = this.unitOfWork.AnwserBlockRepository.GetByID(anwserBlockId);

            if (anwserBlockValidation == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockNotExistant}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
            }

            if (anwserBlockValidation.CompanyID != companyId)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany);
            }

            if (anwserBlockId <= 0)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero);
            }
        }
    }
}
