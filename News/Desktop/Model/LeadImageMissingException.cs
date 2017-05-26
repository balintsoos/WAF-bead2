using System;

namespace Desktop.Model
{
    public class LeadImageMissingException : Exception
    {
        public LeadImageMissingException(String message) : base(message) { }

        public LeadImageMissingException(Exception innerException) : base("Exception occurred.", innerException) { }
    }
}
