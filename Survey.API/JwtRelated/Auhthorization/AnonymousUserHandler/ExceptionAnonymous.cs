namespace Survey.API.JwtRelated.Auhthorization.AnonymousUserHandler
{
    [Serializable]
    public class ExceptionAnonymous : Exception
    {
        private string Unauthorized { get; set; }

        public ExceptionAnonymous()
        { }

        public ExceptionAnonymous(string message) : base(message)
        { }

        public ExceptionAnonymous(string message, Exception inner) : base(message, inner)
        { }

        public ExceptionAnonymous(string message, string unauthorized) : this(message)
        {
            Unauthorized = unauthorized;
        }
    }
}
