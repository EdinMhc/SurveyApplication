namespace Survey.Domain.Services.FluentValidation.AnswerBlock
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerBlockCreationValidation : AbstractValidator<AnwserBlock>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult result;

        public AnswerBlockCreationValidation(IUnitOfWork unitOfWork, int companyId, int surveyId)
        {
            result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            SurveyId = surveyId;

            RuleFor(x => x.AnwserBlockName)
                .NotEmpty()
                .Length(2, 255)
                .NotNull();
            RuleFor(x => x.BlockType)
                .NotNull()
                .Length(2, 255)
                .NotNull();
            RuleFor(x => x.CodeOfAnwserBlock)
                .GreaterThanOrEqualTo(1);
        }

        public override ValidationResult Validate(ValidationContext<AnwserBlock> context)
        {
            result = base.Validate(context);
            ValidateCompany();
            ValidateSurvey();

            return result;
        }

        private void ValidateCompany()
        {
            if (CompanyId <= 0)
            {
                result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyIDvalidation]));
            }

            // Company Existance
            var companyInfo = _unitOfWork.CompanyRepository.GetAll().FirstOrDefault(x => x.CompanyID == CompanyId);
            if (companyInfo == null)
            {
                result.Errors.Add(new ValidationFailure("CompanyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.CompanyNotExistant]));
            }
        }

        private void ValidateSurvey()
        {
            if (SurveyId <= 0)
            {
                result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyIDBelowOrEqualToZero]));
            }

            var dbSurvey = _unitOfWork.SurveysRepository.GetAll().FirstOrDefault(p => p.SurveyID == SurveyId && p.CompanyID == CompanyId);
            if (dbSurvey == null)
            {
                result.Errors.Add(new ValidationFailure("SurveyId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.SurveyNotExistant]));
            }
        }
    }
}
