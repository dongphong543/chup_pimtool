using System;

namespace PIMBackend.Errors
{
    public class StatusInvalidException : BaseException
    {
        string Status;

        public StatusInvalidException() : base() { }

        public StatusInvalidException(string message) : base(message) { }

        public StatusInvalidException(string message, string status) : base(message)
        {
            Status = status;
        }
    }
}
