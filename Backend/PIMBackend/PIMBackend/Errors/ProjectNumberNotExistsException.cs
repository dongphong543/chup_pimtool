using System;

namespace PIMBackend.Errors
{
    public class ProjectNumberNotExistsException : BaseException
    {
        decimal PjNum;

        public ProjectNumberNotExistsException() : base() { }

        public ProjectNumberNotExistsException(string message) : base(message) { }

        public ProjectNumberNotExistsException(string message, decimal pjNum) : base(message)
        {
            PjNum = pjNum;
        }

    }
}
