namespace Survey.Domain.Services.IdentityService.Contracts
{
    public enum ResponseStatusCode
    {
        // [EnumMember(Value = "SUCCESS")]
        Success = 0,

        // [EnumMember(Value = "ERROR")]
        Error = 1,

        // [EnumMember(Value = "VALIDATION_ERROR")]
        ValidationError = 2,

        // [EnumMember(Value = "BAD_REQUEST")]
        BadRequest = 3,

        // [EnumMember(Value = "ACCESS_DENIED_ERROR")]
        AccessDeniedError = 4,

        // [EnumMember(Value = "NOT_FOUND_ERROR")]
        NotFoundError = 5,

        // [EnumMember(Value = "PROJECT_LOCKED")]
        ProjectLocked = 6
    }

    public class Status
    {
        public ResponseStatusCode StatusCode { get; set; }

        public List<string> Messages { get; set; } = new List<string>();
    }

    public class Response
    {
        public object Data { get; set; }

        public object ErrorData { get; set; }

        public Status Status { get; set; } = new Status();


        public Response(object data, params string[] messages)
        {
            this.Status.StatusCode = ResponseStatusCode.Success;
            this.Status.Messages.AddRange(messages);
            this.Data = data;
        }

        public Response(ResponseStatusCode statusCode, object errorData, params string[] messages)
        {
            this.Status.StatusCode = statusCode;
            this.Status.Messages.AddRange(messages);
            this.ErrorData = errorData;
        }

        public Response(ResponseStatusCode statusCode, params string[] messages)
        {
            this.Status.StatusCode = statusCode;
            this.Status.Messages.AddRange(messages);
        }

        public static Response Success(object data, params string[] messages)
        {
            return new Response(data, messages);
        }

        public static Response Success()
        {
            return new Response(data: null, "Changes saved successfully");
        }

        public static Response Success(params string[] messages)
        {
            return new Response(data: null, messages);
        }

        public static Response Error(Exception ex, string action = "not provided", params string[] messages)
        {
            // Log.Error(ex, action);
            return Response.Error(messages);
        }

        public static Response Error(params string[] messages)
        {
            return new Response(ResponseStatusCode.Error, messages ?? new string[] { "An error occurred." });
        }

        public static Response AccessDeniedError(params string[] messages)
        {
            return new Response(
                ResponseStatusCode.AccessDeniedError,
                CombineMessages("Access denied error.", messages)
            );
        }

        public static Response NotFoundError(string id = null, params string[] messages)
        {
            return new Response(
                ResponseStatusCode.NotFoundError,
                CombineMessages($"Resource {(id == null ? "" : $"with id '{id}'")} not found.", messages)
            );
        }

        public static Response ValidationError(params string[] messages)
        {
            return new Response(ResponseStatusCode.ValidationError, messages);
        }

        public static Response ValidationError(object validationData, params string[] messages)
        {
            return new Response(ResponseStatusCode.ValidationError, validationData, messages);
        }

        private static string[] CombineMessages(string message, params string[] messages)
        {
            var combinedMessages = new List<string> { message };
            combinedMessages.AddRange(messages);

            return combinedMessages.ToArray();
        }
    }
}
