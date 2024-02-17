namespace huzcodes.Extensions.Exceptions
{
    public class ResultException : Exception
    {
        public string ResultExceptionMessage { get; set; } = string.Empty;
        public int ResultExceptionStatusCode { get; set; }
        /// <summary>
        /// this class ctor, is used while you are thrown ResultException to be handled in the global exception handler
        /// it has two arguments one for the custom message you want to send in your response in case of errors or exceptions
        /// the other one is the status code you want to send in your response in case of errors or exceptions.
        /// </summary>
        /// <param name="message">the message you want to send in your response in case of errors or exceptions.</param>
        /// <param name="statusCode">the status code you want to send in your response in case of errors or exceptions.</param>
        public ResultException(string message, int statusCode)
        {
            ResultExceptionMessage = message;
            ResultExceptionStatusCode = statusCode;
        }
    }
}
