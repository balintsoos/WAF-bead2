using System;

namespace Desktop.Model
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(String message) : base(message) { }

        public ServiceUnavailableException(Exception innerException) : base("Exception occurred.", innerException) { }
    }
}
