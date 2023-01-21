namespace Survey.Domain.Services.FluentValidation.Question
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class QuestioCreateValidator : AbstractValidator<Question>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int AnswerBlockId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult result;

        public QuestioCreateValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int answerBlockId)
        {
            result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            SurveyId = surveyId;
            AnswerBlockId = answerBlockId;

            RuleFor(x => x.AnwserBlockID)
                .NotNull().WithMessage("Can not create Question AnswerBlock with null property")
                .NotEmpty().WithMessage("Can not create Question AnswerBlock with empty property");

            RuleFor(x => x.Code)
                .NotNull().WithMessage("Can not create Question code with null property")
                .NotEmpty().WithMessage("Can not create Question code with empty property")
                .Length(2, 255).WithMessage("Question Code is shorter or longer than required");

            RuleFor(x => x.QuestionText)
                .NotNull().WithMessage("Can not create Question Text with null property")
                .NotEmpty().WithMessage("Can not create Question Text with empty property")
                .Length(2, 255).WithMessage("Question Text is shorter or longer than required");

            RuleFor(x => x.QuestionType)
                .NotNull().WithMessage("Can not create Question Type with null property")
                .NotEmpty().WithMessage("Can not create Question Type with empty property")
                .Length(2, 255).WithMessage("Question Type is shorter or longer than required");
        }

        public override ValidationResult Validate(ValidationContext<Question> context)
        {
            result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();
            ValidateAnswerBlock();

            return result;
        }

        private void ValidateCompany()
        {
            if (CompanyId <= 0)
            {
                result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            var company = _unitOfWork.CompanyRepository.GetByID(CompanyId);

            if (company == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.CompanyNotExistant);
            }
        }

        private void ValidateSurvey()
        {
            if (SurveyId <= 0)
            {
                result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.CompanyID == CompanyId);

            if (resultSurveyCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipCompanySurvey);
            }
        }

        private void ValidateAnswerBlock()
        {
            var anwserBlockCheck = _unitOfWork.AnwserBlockRepository.GetByID(AnswerBlockId);
            if (anwserBlockCheck == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserBlockNotExistant);
            }
        }

    }
}
