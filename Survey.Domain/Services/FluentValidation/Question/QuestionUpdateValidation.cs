namespace Survey.Domain.Services.FluentValidation.Question
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class QuestionUpdateValidation : AbstractValidator<Question>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int AnswerBlockId;
        private readonly int QuestionId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public QuestionUpdateValidation(IUnitOfWork unitOfWork, int companyId, int surveyId, int questionId, int answerBlockId)
        {
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            SurveyId = surveyId;
            QuestionId = questionId;
            AnswerBlockId = answerBlockId;

            RuleFor(x => x.AnwserBlockID)
                .NotEmpty().WithMessage("Can not create Question AnswerBlock with empty property")
                .Unless(x => x.AnwserBlockID <= 0);

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Can not create Question code with empty property")
                .Length(2, 255).WithMessage("Question Code is shorter or longer than required")
                .Unless(x => x.Code == null || x.Code == string.Empty);

            RuleFor(x => x.QuestionText)
                .Length(2, 255).WithMessage("Question Text is shorter or longer than required")
                .Unless(x => x.QuestionText == null || x.QuestionText == string.Empty);

            RuleFor(x => x.QuestionType)
                .Length(2, 255).WithMessage("Question Type is shorter or longer than required")
                .Unless(x => x.QuestionText == string.Empty);
        }

        public override ValidationResult Validate(ValidationContext<Question> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();
            ValidateAnswerBlock();
            ValidateQuestion();

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            var company = _unitOfWork.CompanyRepository.GetByID(CompanyId);
            if (company == null)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateSurvey()
        {
            if (SurveyId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDValidation]));
            }

            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.CompanyID == CompanyId);
            if (resultSurveyCompany == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipCompanySurvey]));
            }
        }

        private void ValidateQuestion()
        {
            if (QuestionId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("QuestionId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.QuestionIDBelowOrEqualToZero]));
            }

            var resultSurveyQuestion1 = _unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.QuestionID == QuestionId);
            if (resultSurveyQuestion1 == null)
            {
                _result.Errors.Add(new ValidationFailure("QuestionId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.QuestionNotExistant]));
            }

        }

        private void ValidateAnswerBlock()
        {
            if (AnswerBlockId != 0 || AnswerBlockId! < 0)
            {
                var anwserBlockCheck = _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(x => x.AnwserBlockID == AnswerBlockId && x.CompanyID == CompanyId);
                if (anwserBlockCheck == null)
                {
                    _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockIDValidation]));
                }
            }
        }
    }
}
