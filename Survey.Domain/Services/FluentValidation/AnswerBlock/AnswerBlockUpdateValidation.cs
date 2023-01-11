namespace Survey.Domain.Services.FluentValidation.AnswerBlock
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerBlockUpdateValidation : AbstractValidator<AnwserBlock>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int anwserBlockId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public AnswerBlockUpdateValidation(IUnitOfWork unitOfWork, int companyId, int surveyId, int anwserBlockId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.anwserBlockId = anwserBlockId;

            this.RuleFor(x => x.AnwserBlockName)
                .Length(2, 255).WithMessage("AnswerBlock Name is shorter or longer than required")
                .Unless(x => x.AnwserBlockName == null || x.AnwserBlockName == string.Empty);

            this.RuleFor(x => x.BlockType)
                .Length(2, 255).WithMessage("BlockType is shorter or longer than required")
                .Unless(x => x.BlockType == null || x.BlockType == string.Empty);

            this.RuleFor(x => x.CodeOfAnwserBlock)
                .GreaterThanOrEqualTo(1)
                .Unless(x => x.CodeOfAnwserBlock <= 0);
        }

        public override ValidationResult Validate(ValidationContext<AnwserBlock> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
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

        private void ValidateAnswerBlock()
        {
            if (this.anwserBlockId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockCodeCanNotBeZeroOrBelow]));
            }

            var dbAnwserBlock = this.unitOfWork.AnwserBlockRepository.GetByID(this.anwserBlockId);
            if (dbAnwserBlock == null)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockNotExistant]));
            }
        }
    }
}
