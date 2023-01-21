using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Survey.Domain.CustomException;
using Survey.Domain.Services.FluentValidation.Company;
using Survey.Domain.Services.Interfaces;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Survey.Domain.Services
{

    public class CompanyService : ICompanyService
    {
        private readonly ILogger<CompanyService> _logger;
        private IUnitOfWork _unitOfWork;

        public CompanyService(
            IUnitOfWork unitOfWork,
            ILogger<CompanyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Gets all the companies
        /// </summary>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public IEnumerable<Company> GetAll(string? role, string? userId)
        {
            bool isAdmin = role == AdminHelper.Admin;
            var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().Include("Surveys").Where(x => x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetAll().Include("Surveys");

            if (dbCompany == null)
            {
                _logger.LogError($"Error occurred: {ErrorResponseCode.NoResultsOrUserMismatch}");
                throw new CustomException.CustomException(ErrorResponseCode.NoResultsOrUserMismatch);
            }

            return dbCompany;
        }

        /// <summary>
        /// Gets a company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public Company GetById(int id, string? role, string? userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                if (id <= 0)
                {
                    throw new CustomException.CustomException(ErrorResponseCode.CompanyIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id);
                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {ErrorResponseCode.NoResultsOrUserMismatch}");
                    throw new CustomException.CustomException(ErrorResponseCode.NoResultsOrUserMismatch);
                }

                return dbCompany;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(ErrorResponseCode.GlobalError);
            }
        }

        /// <summary>
        /// Creates a company
        /// </summary>
        /// <param name="company"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public async Task<Company> CreateAsync(Company company, string? role, string userId)
        {
            try
            {
                var result = new CompanyValidator(_unitOfWork).Validate(company);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var userCheck = _unitOfWork.UserRepository.GetByID(userId);
                company.User = userCheck;
                company.CreateDate = DateTime.Now;

                _unitOfWork.CompanyRepository.Add(company);
                await _unitOfWork.SaveChangesAsync();

                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                if (ex is ValidationException) throw ex;
                throw new CustomException.CustomException(ErrorResponseCode.GlobalError);
            }

        }

        /// <summary>
        /// Deletes a company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public async Task<bool> DeleteAsync(int id, string? role, string? userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;
                if (id <= 0)
                {
                    throw new CustomException.CustomException(ErrorResponseCode.CompanyIDBelowOrEqualToZero);
                }

                var dbCompany = isAdmin
                ? _unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id && x.User.Id == userId)
                : _unitOfWork.CompanyRepository.GetAll().Include("Surveys").FirstOrDefault(x => x.CompanyID == id);

                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {ErrorResponseCode.NoResultsOrUserMismatch}");
                    throw new CustomException.CustomException(ErrorResponseCode.NoResultsOrUserMismatch);
                }

                _unitOfWork.CompanyRepository.Delete(dbCompany);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;

                throw new CustomException.CustomException(ErrorResponseCode.GlobalError);
            }
        }

        /// <summary>
        /// Updates a company
        /// </summary>
        /// <param name="company"></param>
        /// <param name="role"></param>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException.CustomException"></exception>
        public async Task<Company> UpdateAsync(Company company, string? role, int companyId, string? userId)
        {
            try
            {
                bool isAdmin = role == AdminHelper.Admin;

                var result = new UpdateValidatorCompany(_unitOfWork, companyId).Validate(company);
                if (!result.IsValid)
                {
                    throw new CustomException.CustomException(string.Join(",\n", result.Errors.Select(x => x.ErrorMessage)));
                }

                var dbCompany = isAdmin
                    ? _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == companyId && x.User.Id == userId)
                    : _unitOfWork.CompanyRepository.GetByID(companyId);

                if (dbCompany == null)
                {
                    _logger.LogError($"Error occurred: {ErrorResponseCode.NoResultsOrUserMismatch}");
                    throw new CustomException.CustomException(ErrorResponseCode.NoResultsOrUserMismatch);
                }

                dbCompany.CompanyName = !string.IsNullOrWhiteSpace(company.CompanyName) ? company.CompanyName : dbCompany.CompanyName;
                dbCompany.Email = !string.IsNullOrWhiteSpace(company.Email) ? company.Email : dbCompany.Email;
                dbCompany.Address = !string.IsNullOrWhiteSpace(company.Address) ? company.Address : dbCompany.Address;

                _unitOfWork.CompanyRepository.Update(dbCompany);
                await _unitOfWork.SaveChangesAsync();

                return dbCompany;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex}");
                if (ex is CustomException.CustomException) throw ex;
                throw new CustomException.CustomException(ErrorResponseCode.GlobalError);
            }
        }
    }
}
