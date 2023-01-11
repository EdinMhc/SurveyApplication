namespace Survey.Domain.DapperServices.DapperCompanyServices
{
    using Microsoft.Extensions.Logging;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Repositories;

    public class CompanyServiceDapper : ICompanyServiceDapper
    {
        private readonly ILogger<CompanyServiceDapper> logger;
        private IUnitOfWork unitOfWork;

        public CompanyServiceDapper(IUnitOfWork unitOfWork, ILogger<CompanyServiceDapper> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        // ------------------------ GetAll ------------------------
        public async Task<IEnumerable<Company>> GetAllAsync(string? role, string userId)
        {
            // CompanyAdmin Access
            if (role == AdminHelper.Admin)
            {
                var result = await this.unitOfWork.companyGenericRepository.GetAllAsync();
                return result;
            }

            // SuperAdmin Access
            var result1 = await this.unitOfWork.companyGenericRepository.GetAllAsync();
            return result1;
        }

        // ------------------------ GetByID ------------------------
        public async Task<Company> GetById(int CompanyId, string? role, string? userId)
        {
            var userCheck = this.unitOfWork.CompanyRepository.GetAll().Where(x => x.CompanyID == CompanyId && x.UserID == userId);
            if (userCheck == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
            }

            if (role == AdminHelper.Admin)
            {
                var result = await this.unitOfWork.companyGenericRepository.GetAsync(CompanyId);
                return result;
            }

            var result1 = await this.unitOfWork.companyGenericRepository.GetAsync(CompanyId);
            return result1;
        }

        public async Task<Company> UpdateDapper(List<DapperCompanyCreationDto> company, string? role, string userId)
        {
            try
            {
                // CompanyAdmin Access
                if (role == AdminHelper.Admin)
                {
                    var userCheck = this.unitOfWork.UserRepository.GetByID(userId);
                    if (userCheck == null)
                    {
                        throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                    }

                    // Transform 1 object result into a list

                    await this.unitOfWork.companyGenericRepository.StoredProcedureAsync(company);
                    await this.unitOfWork.SaveChangesAsync();

                    return null;
                }

                // SuperAdmin Access
                var userCheck1 = this.unitOfWork.UserRepository.GetByID(userId);
                if (userCheck1 == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                // Transform 1 object result into a list
                // Company company1 = new Company();
                // List<Company> transform = new List<Company> { company1 };

                await this.unitOfWork.companyGenericRepository.StoredProcedureAsync(company);
                await this.unitOfWork.SaveChangesAsync();

                return null;
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
            //try
            //{
            //    if (role == AdminHelper.Admin)
            //    {
            //        var userCheck = this.unitOfWork.UserRepository.GetByID(userId);
            //        if (userCheck == null)
            //        {
            //            throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
            //        }

            //        // company.User = userCheck;
            //        company.UserID = userId;
            //        company.CreateDate = DateTime.Now;
            //        var getCompanies = this.unitOfWork.CompanyRepository.GetAll().ToList();

            //        Company take = getCompanies.FirstOrDefault(x => x.CompanyID == 1);

            //        // Transform 1 object result into a list
            //        List<Company> getCompanySingle = new List<Company> { take };

            //        await this.unitOfWork.companyGenericRepository.StoredProcedureAsync(getCompanySingle);
            //        await this.unitOfWork.SaveChangesAsync();
            //    }

            //    var userCheck1 = this.unitOfWork.UserRepository.GetByID(userId);
            //    if (userCheck1 == null)
            //    {
            //        throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
            //    }

            //    company.User = userCheck1;
            //    company.CreateDate = DateTime.Now;

            //    await this.unitOfWork.companyGenericRepository.InsertAsync(company);
            //    await this.unitOfWork.SaveChangesAsync();

            //    return company;
            //}
            //catch (Exception ex)
            //{
            //    this.logger.LogError($"Error occurred: {ex}");
            //    if (ex is CustomException.CustomException) throw ex;

            //    throw new CustomException.CustomException(CustomException.ErrorResponseCode.GlobalError);
            //}
            throw new NotImplementedException();
        }

        public async Task<Company> UpdateAsync(Company company, string? role, int companyId, string? userId)
        {
            try
            {
                if (role == AdminHelper.Admin)
                {
                    // CompanyAdmin Access
                    var dbCompany1 = this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId);
                    if (dbCompany1 == null)
                    {
                        this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyNotExistant}");
                        throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
                    }

                    dbCompany1.CompanyName = company.CompanyName ?? dbCompany1.CompanyName;
                    dbCompany1.Email = company.Email ?? dbCompany1.Email;
                    dbCompany1.Address = company.Address ?? dbCompany1.Address;

                    await this.unitOfWork.companyGenericRepository.UpdateAsync(dbCompany1);
                    await this.unitOfWork.SaveChangesAsync();

                    return dbCompany1;
                }

                // SuperAdmin Access
                var dbCompany = this.unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    this.logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyNotExistant}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
                }

                dbCompany.CompanyName = company.CompanyName ?? dbCompany.CompanyName;
                dbCompany.Email = company.Email ?? dbCompany.Email;
                dbCompany.Address = company.Address ?? dbCompany.Address;

                await this.unitOfWork.companyGenericRepository.UpdateAsync(dbCompany);
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

        public Task<bool> DeleteAsync(int companyId, string? role, string? userId)
        {
            throw new NotImplementedException();
        }


    }
}
