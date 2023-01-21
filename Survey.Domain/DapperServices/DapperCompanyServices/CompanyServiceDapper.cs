using Microsoft.Extensions.Logging;
using Survey.Domain.Services;
using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Repositories;

namespace Survey.Domain.DapperServices.DapperCompanyServices
{

    public class CompanyServiceDapper : ICompanyServiceDapper
    {
        private readonly ILogger<CompanyServiceDapper> _logger;
        private IUnitOfWork _unitOfWork;

        public CompanyServiceDapper(IUnitOfWork unitOfWork, ILogger<CompanyServiceDapper> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // ------------------------ GetAll ------------------------
        public async Task<IEnumerable<Company>> GetAllAsync(string? role, string userId)
        {
            // CompanyAdmin Access
            if (role == AdminHelper.Admin)
            {
                var adminCompany = await _unitOfWork.companyGenericRepository.GetAllAsync();
                return adminCompany;
            }

            // SuperAdmin Access
            var superAdminCompany = await _unitOfWork.companyGenericRepository.GetAllAsync();
            return superAdminCompany;
        }

        // ------------------------ GetByID ------------------------
        public async Task<Company> GetById(int CompanyId, string? role, string? userId)
        {
            var userCheck = _unitOfWork.CompanyRepository.GetAll().Where(x => x.CompanyID == CompanyId && x.UserID == userId);
            if (userCheck == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
            }

            if (role == AdminHelper.Admin)
            {
                var AdminCompany = await _unitOfWork.companyGenericRepository.GetAsync(CompanyId);
                return AdminCompany;
            }

            var superAdminCompany = await _unitOfWork.companyGenericRepository.GetAsync(CompanyId);
            return superAdminCompany;
        }

        public async Task<Company> UpdateDapper(List<DapperCompanyCreationDto> company, string? role, string userId)
        {
            try
            {
                // CompanyAdmin Access
                if (role == AdminHelper.Admin)
                {
                    var isAdmin = _unitOfWork.UserRepository.GetByID(userId);
                    if (isAdmin == null)
                    {
                        throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                    }

                    // Transform 1 object result into a list

                    await _unitOfWork.companyGenericRepository.StoredProcedureAsync(company);
                    await _unitOfWork.SaveChangesAsync();

                    return null;
                }

                // SuperAdmin Access
                var isSuperAdmin = _unitOfWork.UserRepository.GetByID(userId);
                if (isSuperAdmin == null)
                {
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
                }

                // Transform 1 object result into a list
                // Company company1 = new Company();
                // List<Company> transform = new List<Company> { company1 };

                await _unitOfWork.companyGenericRepository.StoredProcedureAsync(company);
                await _unitOfWork.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
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
            //        var userCheck = unitOfWork.UserRepository.GetByID(userId);
            //        if (userCheck == null)
            //        {
            //            throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
            //        }

            //        // company.User = userCheck;
            //        company.UserID = userId;
            //        company.CreateDate = DateTime.Now;
            //        var getCompanies = unitOfWork.CompanyRepository.GetAll().ToList();

            //        Company take = getCompanies.FirstOrDefault(x => x.CompanyID == 1);

            //        // Transform 1 object result into a list
            //        List<Company> getCompanySingle = new List<Company> { take };

            //        await unitOfWork.companyGenericRepository.StoredProcedureAsync(getCompanySingle);
            //        await unitOfWork.SaveChangesAsync();
            //    }

            //    var userCheck1 = unitOfWork.UserRepository.GetByID(userId);
            //    if (userCheck1 == null)
            //    {
            //        throw new CustomException.CustomException(CustomException.ErrorResponseCode.UserDoesNotMatch);
            //    }

            //    company.User = userCheck1;
            //    company.CreateDate = DateTime.Now;

            //    await unitOfWork.companyGenericRepository.InsertAsync(company);
            //    await unitOfWork.SaveChangesAsync();

            //    return company;
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError($"Error occurred: {ex}");
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
                    var dbCompany1 = _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId);
                    if (dbCompany1 == null)
                    {
                        _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyNotExistant}");
                        throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
                    }

                    dbCompany1.CompanyName = company.CompanyName ?? dbCompany1.CompanyName;
                    dbCompany1.Email = company.Email ?? dbCompany1.Email;
                    dbCompany1.Address = company.Address ?? dbCompany1.Address;

                    await _unitOfWork.companyGenericRepository.UpdateAsync(dbCompany1);
                    await _unitOfWork.SaveChangesAsync();

                    return dbCompany1;
                }

                // SuperAdmin Access
                var dbCompany = _unitOfWork.CompanyRepository.GetByID(companyId);
                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {CustomException.ErrorResponseCode.CompanyNotExistant}");
                    throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
                }

                dbCompany.CompanyName = company.CompanyName ?? dbCompany.CompanyName;
                dbCompany.Email = company.Email ?? dbCompany.Email;
                dbCompany.Address = company.Address ?? dbCompany.Address;

                await _unitOfWork.companyGenericRepository.UpdateAsync(dbCompany);
                await _unitOfWork.SaveChangesAsync();

                return dbCompany;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
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
