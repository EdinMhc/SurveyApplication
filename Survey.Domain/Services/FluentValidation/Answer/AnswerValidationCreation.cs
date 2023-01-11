namespace Survey.Domain.Services.FluentValidation.Answer
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerValidationCreation : AbstractValidator<Anwser>
    {
        private readonly int companyId;
        private readonly int anwserBlockId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public AnswerValidationCreation(IUnitOfWork unitOfWork, int companyId, int anwserBlockId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.anwserBlockId = anwserBlockId;
            this.companyId = companyId;

            this.RuleFor(x => x.AnwserText)
                .NotNull().WithMessage("Can not create Answer Text with null property")
                .NotEmpty().WithMessage("Can not create Answer Text with empty property");
        }

        public override ValidationResult Validate(ValidationContext<Anwser> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateAnswerBlock();

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            // Company Existance
            var companyInfo = this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == this.companyId);
            if (companyInfo == null)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateAnswerBlock()
        {
            if (this.anwserBlockId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero]));
            }

            var anwserBlock1 = this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == this.anwserBlockId && x.CompanyID == this.companyId);
            if (anwserBlock1 == null)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany]));
            }
        }
    }
}
