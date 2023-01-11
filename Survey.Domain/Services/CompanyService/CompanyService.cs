namespace Survey.Domain.Services.CompanyService
{
    using global::FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.FluentValidation.Company;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class CompanyService : ICompanyService
    {
        private readonly ILogger<CompanyService> logger;
        private IUnitOfWork unitOfWork;

        public CompanyService(
            IUnitOfWork unitOfWork,
            ILogger<CompanyService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public IEnumerable<Company> GetAll(string? role, string? userId)
        {
            bool isAdmin = role == AdminHelper.Admin;
            var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().Include("Surveys").Where(x => x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetAll().Include("Surveys");

            if (dbCompany == null)
            {
                this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.NoResultsOrUserMismatch}");
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
            }

            return dbCompany;
        }

        public Company GetById(int id, string? role, string? userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                if (id <= 0)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id);
                if (dbCompany == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.NoResultsOrUserMismatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                return dbCompany;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<Company> CreateAsync(Company company, string? role, string userId)
        {
            try
            {
                var result = new CompanyValidator(this.unitOfWork).Validate(company);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var userCheck = this.unitOfWork.UserRepository.GetByID(userId);
                company.User = userCheck;
                company.CreateDate = DateTime.Now;

                this.unitOfWork.CompanyRepository.Add(company);
                await this.unitOfWork.SaveChangesAsync();

                return company;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }

        }

        public async Task<bool> DeleteAsync(int id, string? role, string? userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;
                if (id <= 0)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                ? this.unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id && x.User.Id == userId)
                : this.unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id);

                if (dbCompany == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.NoResultsOrUserMismatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                this.unitOfWork.CompanyRepository.Delete(dbCompany);
                await this.unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;

                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }

        public async Task<Company> UpdateAsync(Company company, string? role, int companyId, string? userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new UpdateValidatorCompany(this.unitOfWork, companyId).Validate(company);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(String.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : this.unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.NoResultsOrUserMismatch}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.NoResultsOrUserMismatch);
                }

                dbCompany.CompanyName = !string.IsNullOrWhiteSpace(company.CompanyName) ? company.CompanyName : dbCompany.CompanyName;
                dbCompany.Email = !string.IsNullOrWhiteSpace(company.Email) ? company.Email : dbCompany.Email;
                dbCompany.Address = !string.IsNullOrWhiteSpace(company.Address) ? company.Address : dbCompany.Address;

                this.unitOfWork.CompanyRepository.Update(dbCompany);
                await this.unitOfWork.SaveChangesAsync();

                return dbCompany;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            }
        }
    }
}
