using System;

namespace PIMBackend.Errors
{
    public class VisaInvalidException : Exception
    {
        string Visa;

        public VisaInvalidException() : base() { }

        public VisaInvalidException(string message) : base(message) { }

        public VisaInvalidException(string message, string visa) : base(message)
        {
            Visa = visa;
        }
    }
}
