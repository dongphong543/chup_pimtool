using System;

namespace PIMBackend.Errors
{
    public class PageInvalidException : BaseException
    {
        int PageNumber;

        public PageInvalidException(): base() { }

        public PageInvalidException(string message) : base(message) { }

        public PageInvalidException(string message, int n): base(message)
        {
            PageNumber = n;
        }
        
    }
}
