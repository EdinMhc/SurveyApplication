namespace Survey.Domain.Services.FluentValidation.SurveyReportData
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportDataCreationValidator : AbstractValidator<SurveyReportData>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int SurveyReportId;
        private readonly int QuestionId;
        private readonly int AnswerId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyReportDataCreationValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId, int questionId, int answerId)
        {
            CompanyId = companyId;
            SurveyId = surveyId;
            SurveyReportId = surveyReportId;
            QuestionId = questionId;
            AnswerId = answerId;
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;

            RuleFor(x => x.QuestionID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.AnswerID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }

        public override ValidationResult Validate(ValidationContext<SurveyReportData> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();
            ValidateSurveyReport();
            ValidateAnswer();
            QuestionValidation();
            AnswerBlockValidation(QuestionValidation(), ValidateAnswer());

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId == 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
            }

            var companyCheck = _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == CompanyId);
            if (companyCheck == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateSurvey()
        {
            if (SurveyId == 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var resultSurveyCompany = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.CompanyID == CompanyId);
            if (resultSurveyCompany == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipCompanySurvey]));

            }
        }

        private void ValidateSurveyReport()
        {
            if (SurveyReportId == 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurveyReport = _unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.SurveyReportID == SurveyReportId);
            if (dbSurveyReport == null)
            {
                _result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportNotExistant]));
            }
        }

        private void AnswerBlockValidation(Question question, Anwser answer)
        {
            var answerBlock = _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(p => p.CompanyID == CompanyId);
            if (answerBlock == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany);
            }

            // Connection between ANSWER AND ANSWERBLOCK
            if (answer.AnwserBlockID != answerBlock.AnwserBlockID || answer == null)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockAnswer]));
            }

            // Connection between QUESTION AND ANSWERBLOCK
            if (question.AnwserBlockID != answerBlock.AnwserBlockID)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockQuestion]));
            }
        }

        private Anwser ValidateAnswer()
        {
            var answer = _unitOfWork.AnwserRepository.GetByID(AnswerId);
            if (answer == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);

            }

            return answer;
        }

        private Question QuestionValidation()
        {
            var question = _unitOfWork.QuestionRepository.GetByID(QuestionId);
            if (question == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionIDValidation);
            }

            return question;
        }
    }
}
