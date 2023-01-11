namespace Survey.Domain.Services.FluentValidation.SurveyReportData
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportDataUpdateValidator : AbstractValidator<SurveyReportData>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int surveyReportId;
        private readonly int questionId;
        private readonly int respondentId;
        private readonly int answerId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public SurveyReportDataUpdateValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId, int questionId, int answerId, int respondentId)
        {
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.surveyReportId = surveyReportId;
            this.questionId = questionId;
            this.answerId = answerId;
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;
            this.respondentId = respondentId;

            this.RuleFor(x => x.QuestionID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .Unless(x => x.QuestionID <= 0);

            this.RuleFor(x => x.AnswerID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .Unless(x => x.AnswerID <= 0);
        }

        public override ValidationResult Validate(ValidationContext<SurveyReportData> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
            this.ValidateSurveyReport();
            this.ValidateSurveyReportData();
            this.ValidateAnswer();
            this.ValidateQuestion();
            this.ValidateAnswerBlock(this.ValidateQuestion(), this.ValidateAnswer());

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
            }
        }

        private void ValidateSurvey()
        {
            if (this.surveyId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var resultSurveyCompany = this.unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.CompanyID == this.companyId);
            if (resultSurveyCompany == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipCompanySurvey]));

            }
        }

        private void ValidateSurveyReport()
        {
            if (this.surveyReportId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurveyReport = this.unitOfWork.SurveyReportRepository.GetAll().FirstOrDefault(p => p.SurveyID == this.surveyId && p.SurveyReportID == this.surveyReportId);
            if (dbSurveyReport == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyReportId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportNotExistant]));
            }

            var resultSurveyReportCompany = this.unitOfWork.SurveyReportDataRepository.GetAll().FirstOrDefault(p => p.SurveyReportID == this.surveyReportId);
            if (resultSurveyReportCompany == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationshipSurveySurvey);
            }

        }

        private void ValidateSurveyReportData()
        {
            if (this.respondentId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyReportDataId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero]));

            }
        }

        private Anwser ValidateAnswer()
        {
            if (this.answerId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserIDBelowOrEqualToZero]));
            }

            var answerCheckForUpdate = this.unitOfWork.AnwserRepository.GetAll().FirstOrDefault(p => p.AnwserID == this.answerId);
            if (answerCheckForUpdate == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserIDValidation);
            }

            return answerCheckForUpdate;
        }

        private Question ValidateQuestion()
        {
            if (this.questionId <= 0)
            {
                this.result.Errors.Add(new ValidationFailure("QuestionId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.QuestionIDBelowOrEqualToZero]));
            }

            var questionCheckForUpdate = this.unitOfWork.QuestionRepository.GetAll().FirstOrDefault(p => p.QuestionID == this.questionId);
            if (questionCheckForUpdate == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionIDValidation);
            }

            return questionCheckForUpdate;
        }

        private void ValidateAnswerBlock(Question question, Anwser answer)
        {
            var answerBlock = this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(p => p.CompanyID == this.companyId);
            if (answerBlock == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany);
            }

            // Connection between ANSWER AND ANSWERBLOCK
            if (answer.AnwserBlockID != answerBlock.AnwserBlockID)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockAnswer]));
            }

            // Connection between QUESTION AND ANSWERBLOCK
            if (question.AnwserBlockID != answerBlock.AnwserBlockID)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockQuestion]));
            }
        }
    }
}
