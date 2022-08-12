using System;

namespace PIMBackend.Errors
{
    public class DateInvalidException : BaseException
    {
        DateTime StartDate;
        DateTime EndDate;

        public DateInvalidException(): base() { }

        public DateInvalidException(string message) : base(message) { }

        public DateInvalidException(string message, DateTime s, DateTime e): base(message)
        {
            StartDate = s;
            EndDate = e;
        }
        
    }
}
