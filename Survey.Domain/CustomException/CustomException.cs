namespace Survey.Domain.CustomException
{
    public class CustomException : Exception
    {
        public int ResponseCodeStatus { get; set; }

        public string ErrorMessage { get; set; }

        public CustomException(ErrorResponseCode responseCode)
        {
            this.ResponseCodeStatus = (int)responseCode;
            this.ErrorMessage = Errors.GetValueOrDefault(responseCode, "Error occurred");
        }

        public CustomException(string validationMEssage)
        {

            this.ResponseCodeStatus = (int)ErrorResponseCode.ValidationError;
            this.ErrorMessage = validationMEssage;

        }

        public static string GetErrorMessageByKey(ErrorResponseCode errorResponseCode) => Errors.GetValueOrDefault(errorResponseCode, "Error occurred");

        public static Dictionary<ErrorResponseCode, string> Errors = new Dictionary<ErrorResponseCode, string>()
        {
            {ErrorResponseCode.InternalServerError, "Internal server error" },
            {ErrorResponseCode.ValidationError, "Validation error" },
            {ErrorResponseCode.GlobalError, "Error occurred" },
            {ErrorResponseCode.AnonymousAccessDenied, "Access without a user is not permited" },
            {ErrorResponseCode.RelationshipCompanySurvey, "Company ID and Survey ID do not match" },
            {ErrorResponseCode.Unauthorized, "Unauthorized access" },
            {ErrorResponseCode.RelationShipAnswerBlockCompany, "AnswerBlockID and CompanyID do not match" },
            {ErrorResponseCode.RelationshipSurveySurvey, "SurveyReportID and SurveyID do not match" },
            {ErrorResponseCode.RelationshipAnwserBlockAnswer, "AnswerBlockID and AnswerID do not match" },
            {ErrorResponseCode.RelationshipAnwserBlockQuestion, "AnswerBlockID and QuestionID do not match" },
            {ErrorResponseCode.CompanyNull, "Data for company are missing" },
            {ErrorResponseCode.CompanyNameNullOrEmpty, "Company name is null or empty" },
            {ErrorResponseCode.CompanyAddressNullOrEmpty, "The Company address is null or empty" },
            {ErrorResponseCode.CompanyIDvalidation, "Company ID is not valid" },
            {ErrorResponseCode.CompanyNotExistant, "Company Does not exist" },
            {ErrorResponseCode.CompanyEmailNotVaild, "Email is invalid" },
            {ErrorResponseCode.CompanyIDBelowOrEqualToZero, "Company ID is below or equal to zero" },
            {ErrorResponseCode.SurveyNull, "Data for Survey are missing" },
            {ErrorResponseCode.SurveyNameNullOrEmpty, "Survey name is null or empty" },
            {ErrorResponseCode.SurveyIDValidation, "Survey ID is not valid" },
            {ErrorResponseCode.SurveyNotExistant, "Survey does not exist" },
            {ErrorResponseCode.SurveyCreatedByNullOrEmpty, "Created by is Null or Empty" },
            {ErrorResponseCode.SurveyIDBelowOrEqualToZero, "Survey ID is below or equal to zero" },
            {ErrorResponseCode.SurveyReportNull, "Data for SurveyReport are missing" },
            {ErrorResponseCode.SurveyReportNameNullOrEmpty, "SurveyReport name is null or empty" },
            {ErrorResponseCode.SurveyReportIDValidation, "SurveyReport ID is not valid" },
            {ErrorResponseCode.SurveyReportNotExistant, "SurveyReport does not exist" },
            {ErrorResponseCode.SurveyReportCreatedByNullOrEmpty, "Created by is Null or Empty" },
            {ErrorResponseCode.SurveyReportIDBelowOrEqualToZero, "SurveyReport ID is below or equal to zero" },
            {ErrorResponseCode.QuestionNull, "Data for Questions are missing" },
            {ErrorResponseCode.QuestionNameNullOrEmpty, "Question name is null or empty" },
            {ErrorResponseCode.QuestionIDValidation, "Question ID is not valid" },
            {ErrorResponseCode.QuestionNotExistant, "Question does not exist" },
            {ErrorResponseCode.QuestionCreatedByNullOrEmpty, "Created by is Null or Empty" },
            {ErrorResponseCode.QuestionIDBelowOrEqualToZero, "Question ID is below or equal to zero" },
            {ErrorResponseCode.QuestionTextIsNullOrEmpty, "Question text is Null or Empty" },
            {ErrorResponseCode.QuestionTypeIsNullOrEmpty, "Question type is Null or Empty" },
            {ErrorResponseCode.AnwserBlockNull, "Data for AnwserBlock are missing" },
            {ErrorResponseCode.AnwserBlockNameNullOrEmpty, "AnwserBlock name is null or empty" },
            {ErrorResponseCode.AnwserBlockIDValidation, "AnwserBlock ID is not valid" },
            {ErrorResponseCode.AnwserBlockNotExistant, "AnwserBlock does not exist" },
            {ErrorResponseCode.AnwserBlockCreatedByNullOrEmpty, "Created by is Null or Empty" },
            {ErrorResponseCode.AnwserBlockIDBelowOrEqualToZero, "AnwserBlock ID is below or equal to zero" },
            {ErrorResponseCode.AnwserBlockCodeCanNotBeZeroOrBelow, "AnwserBlock Code property can not be zero or below zero" },
            {ErrorResponseCode.AnwserNull, "Data for Answer are missing" },
            {ErrorResponseCode.AnwserNameNullOrEmpty, "Answer name is null or empty" },
            {ErrorResponseCode.AnwserIDValidation, "Answer ID is not valid" },
            {ErrorResponseCode.AnwserNotExistant, "Answer does not exist" },
            {ErrorResponseCode.AnwserCreatedByNullOrEmpty, "Created by is Null or Empty" },
            {ErrorResponseCode.AnwserIDBelowOrEqualToZero, "Answer ID is below or equal to zero" },
            {ErrorResponseCode.AnwserCodeCanNotBeZeroOrBelow, "Answer Code property can not be zero or below zero" },
            {ErrorResponseCode.UserDoesNotMatch, "User does not match" },
            {ErrorResponseCode.SurveyReportDataNull, "Data for SurveyReportData are missing" },
            {ErrorResponseCode.SurveyReportDataNameNullOrEmpty, "SurveyReportData name is null or empty" },
            {ErrorResponseCode.SurveyReportDataIDValidation, "SurveyReportData ID is not valid" },
            {ErrorResponseCode.SurveyReportDataNotExistant, "SurveyReportData does not exist" },
            {ErrorResponseCode.SurveyReportDataCreatedByNullOrEmpty, "Created by is Null or Empty" },
            {ErrorResponseCode.SurveyReportDataIDBelowOrEqualToZero, "SurveyReportData ID is below or equal to zero" },
            {ErrorResponseCode.ValidationErrorCompanyName, "Failed to validate, CompanyName is too short. Must be more than 2 characters" },
            {ErrorResponseCode.NoResultsOrUserMismatch, "No results for search. User is a mismatch or Company does not exist" },
        };
    }



    public enum ErrorResponseCode
    {
        InternalServerError = 500,
        GlobalError = 501,
        ValidationError = 502,
        // Company
        CompanyNull = 100,
        CompanyNameNullOrEmpty = 101,
        CompanyAddressNullOrEmpty = 102,
        CompanyIDvalidation = 103,
        CompanyNotExistant = 104,
        CompanyEmailNotVaild = 105,
        CompanyIDBelowOrEqualToZero = 106,
        // Survey
        SurveyNull = 107,
        SurveyNameNullOrEmpty = 108,
        SurveyIDValidation = 109,
        SurveyNotExistant = 110,
        SurveyCreatedByNullOrEmpty = 111,
        SurveyIDBelowOrEqualToZero = 112,
        // SurveyReport
        SurveyReportNull = 113,
        SurveyReportNameNullOrEmpty = 114,
        SurveyReportIDValidation = 115,
        SurveyReportNotExistant = 116,
        SurveyReportCreatedByNullOrEmpty = 117,
        SurveyReportIDBelowOrEqualToZero = 118,
        // Question
        QuestionNull = 119,
        QuestionNameNullOrEmpty = 120,
        QuestionIDValidation = 121,
        QuestionNotExistant = 122,
        QuestionCreatedByNullOrEmpty = 123,
        QuestionIDBelowOrEqualToZero = 124,
        QuestionTextIsNullOrEmpty = 125,
        QuestionTypeIsNullOrEmpty = 126,
        // AnwserBlock
        AnwserBlockNull = 127,
        AnwserBlockNameNullOrEmpty = 128,
        AnwserBlockIDValidation = 129,
        AnwserBlockNotExistant = 130,
        AnwserBlockCreatedByNullOrEmpty = 131,
        AnwserBlockIDBelowOrEqualToZero = 132,
        AnwserBlockCodeCanNotBeZeroOrBelow = 133,
        // Answer
        AnwserNull = 134,
        AnwserNameNullOrEmpty = 135,
        AnwserIDValidation = 136,
        AnwserNotExistant = 137,
        AnwserCreatedByNullOrEmpty = 138,
        AnwserIDBelowOrEqualToZero = 139,
        AnwserCodeCanNotBeZeroOrBelow = 140,
        // Global
        RelationshipCompanySurvey = 141,
        Unauthorized = 142,
        UserDoesNotMatch = 143,
        RelationShipAnswerBlockCompany = 144,
        RelationshipSurveySurvey = 145,
        // SurveyReportData
        SurveyReportDataNull = 146,
        SurveyReportDataNameNullOrEmpty = 147,
        SurveyReportDataIDValidation = 148,
        SurveyReportDataNotExistant = 149,
        SurveyReportDataCreatedByNullOrEmpty = 150,
        SurveyReportDataIDBelowOrEqualToZero = 151,
        SurveyReportDataCodeCanNotBeZeroOrBelow = 152,
        // Globall again
        RelationshipAnwserBlockAnswer = 153,
        RelationshipAnwserBlockQuestion = 154,
        AnonymousAccessDenied = 155,
        ValidationErrorCompanyName = 156,
        NoResultsOrUserMismatch = 157,
    }
}
