using System;

namespace PIMBackend.Errors
{
    public class BaseException: Exception
    {
        public BaseException(): base() { }

        public BaseException(String message) : base(message) { }

        public BaseException(String message, Exception inner) : base(message, inner) { }
    }
}
