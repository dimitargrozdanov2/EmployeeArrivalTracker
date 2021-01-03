using ReportingTool.Data.Utils;
using System;

namespace ReportingTool.Data.Exceptions
{
    public class TokenExpiryException : Exception
    {
        public TokenExpiryException() : base(ErrorConstants.TokenExpiry)
        {
        }

        public TokenExpiryException(string message) : base(message)
        {
        }

        public TokenExpiryException(string message, Exception innerException) : base(message,
             innerException)
        {
        }
    }
}
