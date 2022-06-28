using System;

namespace PIMBackend.Errors
{
    public class FormInvalidException : BaseException
    {
        public FormInvalidException() : base() { }

        public FormInvalidException(string message) : base(message) { }

        

    }
}
