namespace Survey.Domain.Services.FluentValidation.Answer
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerValidationUpdate : AbstractValidator<Anwser>
    {
        private readonly int CompanyId;
        private readonly int AnwserBlockId;
        private readonly int AnswerId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public AnswerValidationUpdate(IUnitOfWork unitOfWork, int companyId, int anwserBlockId, int answerId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            AnwserBlockId = anwserBlockId;
            CompanyId = companyId;
            AnswerId = answerId;

            RuleFor(x => x.AnwserText)
                .Length(1, 255).WithMessage("Question Text is shorter or longer than required")
                .Unless(x => x.AnwserText == null || x.AnwserText == string.Empty);
        }

        public override ValidationResult Validate(ValidationContext<Anwser> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateAnswerBlock();
            ValidateAnswer();

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

            var anwserBlock = _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == AnwserBlockId && x.CompanyID == CompanyId);
            if (anwserBlock == null)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany]));
            }
        }

        private void ValidateAnswer()
        {
            if (AnswerId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("AnswerId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero]));
            }

            var dbAnswer1 = _unitOfWork.AnwserRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == AnwserBlockId && x.AnwserID == AnswerId);

            if (dbAnswer1 == null)
            {
                _result.Errors.Add(new ValidationFailure("AnswerId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserNotExistant]));

            }
        }
    }
}
