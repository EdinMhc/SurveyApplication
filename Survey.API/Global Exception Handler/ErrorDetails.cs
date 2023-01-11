namespace Survey.API.Global_Exception_Handler
{
    using Newtonsoft.Json;
    using Survey.Domain.CustomException;

    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ErrorDetails()
        { }

        public ErrorDetails(ErrorResponseCode statusCode, string message)
        {
            this.StatusCode = (int)statusCode;
            this.Message = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
