namespace Survey.Domain.Services.FluentValidation.AnswerBlock
{
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using global::Survey.Infrastructure.Entities;
    using global::Survey.Infrastructure.Repositories;

    public class AnswerBlockUpdateValidation : AbstractValidator<AnwserBlock>
    {
        private readonly int CompanyId;
        private readonly int SurveyId;
        private readonly int AnwserBlockId;
        private IUnitOfWork _unitOfWork;
        private ValidationResult result;

        public AnswerBlockUpdateValidation(IUnitOfWork unitOfWork, int companyId, int surveyId, int anwserBlockId)
        {
            result = new ValidationResult();
            _unitOfWork = unitOfWork;
            CompanyId = companyId;
            SurveyId = surveyId;
            AnwserBlockId = anwserBlockId;

            RuleFor(x => x.AnwserBlockName)
                .Length(2, 255).WithMessage("AnswerBlock Name is shorter or longer than required")
                .Unless(x => x.AnwserBlockName == null || x.AnwserBlockName == string.Empty);

            RuleFor(x => x.BlockType)
                .Length(2, 255).WithMessage("BlockType is shorter or longer than required")
                .Unless(x => x.BlockType == null || x.BlockType == string.Empty);

            RuleFor(x => x.CodeOfAnwserBlock)
                .GreaterThanOrEqualTo(1)
                .Unless(x => x.CodeOfAnwserBlock <= 0);
        }

        public override ValidationResult Validate(ValidationContext<AnwserBlock> context)
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

        private void ValidateAnswerBlock()
        {
            if (AnwserBlockId <= 0)
            {
                result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockCodeCanNotBeZeroOrBelow]));
            }

            var dbAnwserBlock = _unitOfWork.AnwserBlockRepository.GetByID(AnwserBlockId);
            if (dbAnwserBlock == null)
            {
                result.Errors.Add(new ValidationFailure("AnswerBlockId", CustomException.CustomException.Errors[CustomException.ErrorResponseCode.AnwserBlockNotExistant]));
            }
        }
    }
}
