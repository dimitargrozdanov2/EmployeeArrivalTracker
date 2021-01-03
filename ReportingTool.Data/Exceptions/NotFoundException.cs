using ReportingTool.Data.Utils;
using System;

namespace ReportingTool.Data.Exceptions
{
    public class NotFoundException : Exception
    {

        public NotFoundException() : base(ErrorConstants.NotFound)
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message,
             innerException)
        {
        }
    }
}
