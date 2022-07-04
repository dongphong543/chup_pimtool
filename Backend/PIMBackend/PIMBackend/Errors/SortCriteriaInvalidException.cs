using System;

namespace PIMBackend.Errors
{
    public class SortCriteriaInvalidException : BaseException
    {
        char SortingCol;
        int SortingDirection;

        public SortCriteriaInvalidException(): base() { }

        public SortCriteriaInvalidException(string message) : base(message) { }

        public SortCriteriaInvalidException(string message, char c, int d): base(message)
        {
            SortingCol = c;
            SortingDirection = d;

        }
        
    }
}
