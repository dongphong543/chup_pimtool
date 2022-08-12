using System;

namespace PIMBackend.Errors
{
    public class IdNotExistException : BaseException
    {
        decimal Id;

        public IdNotExistException(): base() { }

        public IdNotExistException(string message) : base(message) { }

        public IdNotExistException(string message, decimal id) : base(message)
        {
            Id = id;
        }
    }
}
