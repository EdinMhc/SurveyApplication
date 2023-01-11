namespace Survey.Domain.Services.FluentValidation.AnswerBlock
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerBlockCreationValidation : AbstractValidator<AnwserBlock>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public AnswerBlockCreationValidation(IUnitOfWork unitOfWork, int companyId, int surveyId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;
            this.surveyId = surveyId;

            this.RuleFor(x => x.AnwserBlockName)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            this.RuleFor(x => x.BlockType)
                .NotNull()
                .Length(2, 255)
                .NotNull();
            this.RuleFor(x => x.CodeOfAnwserBlock)
                .GreaterThanOrEqualTo(1);
        }

        public override ValidationResult Validate(ValidationContext<AnwserBlock> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();

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

        private void ValidateSurvey()
        {
            if (this.surveyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurvey = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.CompanyID == this.companyId);
            if (dbSurvey == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyNotExistant]));
            }
        }
    }
}
