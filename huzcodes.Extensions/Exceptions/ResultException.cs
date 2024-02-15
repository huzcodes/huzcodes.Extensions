namespace huzcodes.Extensions.Exceptions
{
    public class ResultException : Exception
    {
        public string ResultExceptionMessage { get; set; } = string.Empty;
        public int ResultExceptionStatusCode { get; set; }

        public ResultException(string message, int statusCode)
        {
            ResultExceptionMessage = message;
            ResultExceptionStatusCode = statusCode;
        }
    }
}
