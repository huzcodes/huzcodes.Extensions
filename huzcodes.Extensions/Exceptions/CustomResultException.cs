namespace huzcodes.Extensions.Exceptions
{
    public class CustomResultException<TException> : Exception where TException : class
    {
        public TException CustomExceptionContract { get; set; }

        /// <summary>
        /// this class ctor, is used while you are thrown CustomResultException to be handled in the global exception handler,
        /// it has one argument which is generic for any strcuture error or exception response you want to send.
        /// </summary>
        /// <param name="customExceptionContract"></param>
        public CustomResultException(TException customExceptionContract)
        {
            CustomExceptionContract = customExceptionContract;
        }
    }
}
