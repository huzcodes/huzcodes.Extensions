namespace huzcodes.Extensions.API.Models
{
    /// <summary>
    /// this class can be anything with any properties based on your business and what you want to return in case of exception or errors or unsuccess responses.
    /// the properties below are just for example you can add anything as well as the name of the class.
    /// </summary>
    public class CustomExceptionResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string FunctionName { get; set; } = string.Empty;
    }
}
