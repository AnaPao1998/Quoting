using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Exceptions
{
    public class QuoteLogicException : Exception
    {
        public QuoteLogicException(string message) : base(message)
        {

        }
    }
}
