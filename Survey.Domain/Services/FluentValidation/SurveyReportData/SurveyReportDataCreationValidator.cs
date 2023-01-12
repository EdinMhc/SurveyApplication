﻿namespace Survey.Domain.Services.FluentValidation.SurveyReportData
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class SurveyReportDataCreationValidator : AbstractValidator<SurveyReportData>
    {
        private readonly int companyId;
        private readonly int surveyId;
        private readonly int surveyReportId;
        private readonly int questionId;
        private readonly int answerId;
        private IUnitOfWork unitOfWork;
        private ValidationResult result;

        public SurveyReportDataCreationValidator(IUnitOfWork unitOfWork, int companyId, int surveyId, int surveyReportId, int questionId, int answerId)
        {
            this.companyId = companyId;
            this.surveyId = surveyId;
            this.surveyReportId = surveyReportId;
            this.questionId = questionId;
            this.answerId = answerId;
            this.result = new ValidationResult();
            this.unitOfWork = unitOfWork;

            this.RuleFor(x => x.QuestionID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            this.RuleFor(x => x.AnswerID)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(1);
        }

        public override ValidationResult Validate(ValidationContext<SurveyReportData> context)
        {
            this.result = base.Validate(context);
            this.ValidateCompany();
            this.ValidateSurvey();
            this.ValidateSurveyReport();
            this.ValidateAnswer();
            this.QuestionValidation();
            this.AnswerBlockValidation(this.QuestionValidation(), this.ValidateAnswer());

            return this.result;
        }

        private void ValidateCompany()
        {
            if (this.companyId == 0)
            {
                this.result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDBelowOrEqualToZero]));
            }

            var companyCheck = this.unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == this.companyId);
            if (companyCheck == null)
            {
                this.result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
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
        }

        private void AnswerBlockValidation(Question question, Anwser answer)
        {
            var answerBlock = this.unitOfWork.AnwserBlockRepository.GetAll().FirstOrDefault(p => p.CompanyID == this.companyId);
            if (answerBlock == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.RelationShipAnswerBlockCompany);
            }

            // Connection between ANSWER AND ANSWERBLOCK
            if (answer.AnwserBlockID != answerBlock.AnwserBlockID || answer == null)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockAnswer]));
            }

            // Connection between QUESTION AND ANSWERBLOCK
            if (question.AnwserBlockID != answerBlock.AnwserBlockID)
            {
                this.result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.RelationshipAnwserBlockQuestion]));
            }
        }

        private Anwser ValidateAnswer()
        {
            var answer = this.unitOfWork.AnwserRepository.GetByID(this.answerId);
            if (answer == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.AnwserNotExistant);

            }

            return answer;
        }

        private Question QuestionValidation()
        {
            var question = this.unitOfWork.QuestionRepository.GetByID(this.questionId);
            if (question == null)
            {
                throw new CustomException.CustomException(CustomException.ErrorResponseCode.QuestionIDValidation);
            }

            return question;
        }
    }
}