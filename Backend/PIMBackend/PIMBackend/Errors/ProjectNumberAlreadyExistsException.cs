using System;

namespace PIMBackend.Errors
{
    public class ProjectNumberAlreadyExistsException: BaseException
    {
        public decimal PjNum;
        public ProjectNumberAlreadyExistsException() : base() { }

        public ProjectNumberAlreadyExistsException(string message) : base(message) { }

        public ProjectNumberAlreadyExistsException(string message, decimal pjNum) : base(message)
        {
            PjNum = pjNum;
        }
    }
}
