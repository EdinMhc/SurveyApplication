namespace Survey.Domain.Services.FluentValidation.SurveyReportData
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportDataUpdateValidator : AbstractValidator<SurveyReportData>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int SurveyReportId;
        private readonly int QuestionId;
        private readonly int RespondentId;
        private readonly int AnswerId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult _result;

        public SurveyReportDataUpdateValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId, int questionId, int answerId, int respondentId)
        {
            CompanyId = companyId;
            SurveyId = surveyId;
            SurveyReportId = surveyReportId;
            QuestionId = questionId;
            AnswerId = answerId;
            _result = new ValidationResult();
            _unitOfWork = unitOfWork;
            RespondentId = respondentId;

            RuleFor(x => x.QuestionID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .Unless(x => x.QuestionID <= 0);

            RuleFor(x => x.AnswerID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .Unless(x => x.AnswerID <= 0);
        }

        public override ValidationResult Validate(ValidationContext<SurveyReportData> context)
        {
            _result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();
            ValidateSurveyReport();
            ValidateSurveyReportData();
            ValidateAnswer();
            ValidateQuestion();
            ValidateAnswerBlock(ValidateQuestion(), ValidateAnswer());

            return _result;
        }

        private void ValidateCompany()
        {
            if (CompanyId == 0)
            {
                _result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
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

            var resultSurveyReportCompany = _unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.SurveyReportID == SurveyReportId);
            if (resultSurveyReportCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipSurveySurvey);
            }

        }

        private void ValidateSurveyReportData()
        {
            if (RespondentId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("SurveyReportDataId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero]));

            }
        }

        private Anwser ValidateAnswer()
        {
            if (AnswerId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("AnswerId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero]));
            }

            var answerCheckForUpdate = _unitOfWork.AnwserRepository.GetAll().FirstOrDefault(p => p.AnwserID == AnswerId);
            if (answerCheckForUpdate == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserIDValidation);
            }

            return answerCheckForUpdate;
        }

        private Question ValidateQuestion()
        {
            if (QuestionId <= 0)
            {
                _result.Errors.Add(new ValidationFailure("QuestionId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.QuestionIDBelowOrEqualToZero]));
            }

            var questionCheckForUpdate = _unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.QuestionID == QuestionId);
            if (questionCheckForUpdate == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionIDValidation);
            }

            return questionCheckForUpdate;
        }

        private void ValidateAnswerBlock(Question question, Anwser answer)
        {
            var answerBlock = _unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(p => p.CompanyID == CompanyId);
            if (answerBlock == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany);
            }

            // Connection between ANSWER AND ANSWERBLOCK
            if (answer.AnwserBlockID != answerBlock.AnwserBlockID)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockAnswer]));
            }

            // Connection between QUESTION AND ANSWERBLOCK
            if (question.AnwserBlockID != answerBlock.AnwserBlockID)
            {
                _result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockQuestion]));
            }
        }
    }
}
