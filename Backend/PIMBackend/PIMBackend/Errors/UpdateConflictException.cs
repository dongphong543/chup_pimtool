using System;

namespace PIMBackend.Errors
{
    public class UpdateConflictException : BaseException
    {
        public UpdateConflictException(string message, Exception inner) : base(message, inner) { }
    }
}
