namespace Survey.Domain.Services.FluentValidation.Question
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class QuestionUpdateValidation : AbstractValidator<Question>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int answerBlockId;
        private readonly int questionId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public QuestionUpdateValidation(IUnitOfWork unitOfWork, int companyId, int surveyId, int questionId, int answerBlockId)
        {
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.questionId = questionId;
            this.answerBlockId = answerBlockId;

            this.RuleFor(x => x.AnwserBlockID)
                .NotEmpty().WithMessage("Can not create Question AnswerBlock with empty property")
                .Unless(x => x.AnwserBlockID <= 0);

            this.RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Can not create Question code with empty property")
                .Length(2, 255).WithMessage("Question Code is shorter or longer than required")
                .Unless(x => x.Code == null || x.Code == string.Empty);

            this.RuleFor(x => x.QuestionText)
                .Length(2, 255).WithMessage("Question Text is shorter or longer than required")
                .Unless(x => x.QuestionText == null || x.QuestionText == string.Empty);

            this.RuleFor(x => x.QuestionType)
                .Length(2, 255).WithMessage("Question Type is shorter or longer than required")
                .Unless(x => x.QuestionText == string.Empty);
        }

        public override ValidationResult Validate(ValidationContext<Question> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
            this.ValidateAnswerBlock();
            this.ValidateQuestion();

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            var company = this.unitOfWork.CompanyRepository.GetByID(this.companyId);
            if (company == null)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateSurvey()
        {
            if (this.surveyId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDValidation]));
            }

            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.CompanyID == this.companyId);
            if (resultSurveyCompany == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipCompanySurvey]));
            }
        }

        private void ValidateQuestion()
        {
            if (this.questionId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("QuestionId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.QuestionIDBelowOrEqualToZero]));
            }

            var resultSurveyQuestion1 = this.unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.QuestionID == this.questionId);
            if (resultSurveyQuestion1 == null)
            {
                this.result.Errors.Add(new ValidationFailure("QuestionId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.QuestionNotExistant]));
            }

        }

        private void ValidateAnswerBlock()
        {
            if (this.answerBlockId != 0 || this.answerBlockId! < 0)
            {
                var anwserBlockCheck = this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == this.answerBlockId && x.CompanyID == this.companyId);
                if (anwserBlockCheck == null)
                {
                    this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockIDValidation]));
                }
            }
        }
    }
}
