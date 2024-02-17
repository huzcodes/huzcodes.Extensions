namespace huzcodes.Extensions.Exceptions
{
    public class CustomResultException : Exception
    {
        public dynamic CustomExceptionContract { get; set; }

        /// <summary>
        /// this class ctor, is used while you are thrown CustomResultException to be handled in the global exception handler,
        /// it has one argument which is generic for any strcuture error or exception response you want to send.
        /// </summary>
        /// <param name="customExceptionContract"></param>
        public CustomResultException(dynamic customExceptionContract)
        {
            CustomExceptionContract = customExceptionContract;
        }
    }
}
