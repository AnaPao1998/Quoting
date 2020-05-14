using System;
using System.Collections.Generic;
using System.Text;

namespace BackingServices.Exceptions
{
    public class BackingServiceException : Exception
    {
        public BackingServiceException(string message) : base(message) 
        {

        }
    }
}
