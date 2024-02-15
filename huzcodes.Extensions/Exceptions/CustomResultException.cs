namespace huzcodes.Extensions.Exceptions
{
    public class CustomResultException<TException> : Exception where TException : class
    {
        public TException CustomExceptionContract { get; set; }

        public CustomResultException(TException customExceptionContract)
        {
            CustomExceptionContract = customExceptionContract;
        }
    }
}
