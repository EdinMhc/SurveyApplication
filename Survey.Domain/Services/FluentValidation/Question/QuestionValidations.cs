namespace Survey.Domain.Services.FluentValidation.Question
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class QuestioCreateValidator : AbstractValidator<Question>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int answerBlockId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public QuestioCreateValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int answerBlockId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.answerBlockId = answerBlockId;

            this.RuleFor(x => x.AnwserBlockID)
                .NotNull().WithMessage("Can not create Question AnswerBlock with null property")
                .NotEmpty().WithMessage("Can not create Question AnswerBlock with empty property");

            this.RuleFor(x => x.Code)
                .NotNull().WithMessage("Can not create Question code with null property")
                .NotEmpty().WithMessage("Can not create Question code with empty property")
                .Length(2, 255).WithMessage("Question Code is shorter or longer than required");

            this.RuleFor(x => x.QuestionText)
                .NotNull().WithMessage("Can not create Question Text with null property")
                .NotEmpty().WithMessage("Can not create Question Text with empty property")
                .Length(2, 255).WithMessage("Question Text is shorter or longer than required");

            this.RuleFor(x => x.QuestionType)
                .NotNull().WithMessage("Can not create Question Type with null property")
                .NotEmpty().WithMessage("Can not create Question Type with empty property")
                .Length(2, 255).WithMessage("Question Type is shorter or longer than required");
        }

        public override ValidationResult Validate(ValidationContext<Question> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
            this.ValidateAnswerBlock();

            return this.result;
        }

        private void ValidateCompany()
        {
            //TODO DB VALIDATE
            if (this.companyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            var company = this.unitOfWork.CompanyRepository.GetByID(this.companyId);

            if (company == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
            }
        }

        private void ValidateSurvey()
        {
            //TODO DB VALIDATE
            if (this.surveyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.CompanyID == this.companyId);

            if (resultSurveyCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }
        }

        private void ValidateAnswerBlock()
        {
            var anwserBlockCheck = this.unitOfWork.AnwserBlockRepository.GetByID(this.answerBlockId);
            if (anwserBlockCheck == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
            }
        }

    }
}
