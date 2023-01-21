namespace Survey.Domain.Services.FluentValidation.Answer
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerValidationCreation : AbstractValidator<Anwser>
    {
        private readonly int CompanyId;
        private readonly int AnwserBlockId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public AnswerValidationCreation(IUnitOfWork unitOfWork, int companyId, int anwserBlockId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            AnwserBlockId = anwserBlockId;
            CompanyId = companyId;

            RuleFor(x => x.AnwserText)
                .NotNull().WithMessage("Can not create Answer Text with null property")
                .NotEmpty().WithMessage("Can not create Answer Text with empty property");
        }

        public override ValidationResult Validate(ValidationContext<Anwser> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateAnswerBlock();

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            // Company Existance
            var companyInfo = _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == CompanyId);
            if (companyInfo == null)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateAnswerBlock()
        {
            if (AnwserBlockId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero]));
            }

            var anwserBlock1 = _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == AnwserBlockId && x.CompanyID == CompanyId);
            if (anwserBlock1 == null)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany]));
            }
        }
    }
}
