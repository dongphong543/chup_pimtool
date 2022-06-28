using System;

namespace PIMBackend.Errors
{
    public class IdAlreadyExistException : BaseException
    {
        public decimal Id;
        public IdAlreadyExistException() : base() { }

        public IdAlreadyExistException(string message) : base(message) { }

        public IdAlreadyExistException(string message, decimal id) : base(message)
        {
            Id = id;
        }

    }
}
