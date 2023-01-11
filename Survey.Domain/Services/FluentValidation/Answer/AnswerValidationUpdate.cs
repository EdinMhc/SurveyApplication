namespace Survey.Domain.Services.FluentValidation.Answer
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerValidationUpdate : AbstractValidator<Anwser>
    {
        private readonly int companyId;
        private readonly int anwserBlockId;
        private readonly int answerId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public AnswerValidationUpdate(IUnitOfWork unitOfWork, int companyId, int anwserBlockId, int answerId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.anwserBlockId = anwserBlockId;
            this.companyId = companyId;
            this.answerId = answerId;

            this.RuleFor(x => x.AnwserText)
                .Length(1, 255).WithMessage("Question Text is shorter or longer than required")
                .Unless(x => x.AnwserText == null || x.AnwserText == string.Empty);
        }

        public override ValidationResult Validate(ValidationContext<Anwser> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateAnswerBlock();
            this.ValidateAnswer();

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

            var anwserBlock = this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == this.anwserBlockId && x.CompanyID == this.companyId);
            if (anwserBlock == null)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany]));
            }
        }

        private void ValidateAnswer()
        {
            if (this.answerId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero]));
            }

            var dbAnswer1 = this.unitOfWork.AnwserRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == this.anwserBlockId && x.AnwserID == this.answerId);

            if (dbAnswer1 == null)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserNotExistant]));

            }
        }
    }
}
