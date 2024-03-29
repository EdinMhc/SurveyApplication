﻿namespace Survey.API.Global_Exception_Handler
{

    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ErrorDetails()
        { }

        public ErrorDetails(ErrorResponseCode statusCode, string message)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
