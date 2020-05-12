using System;
using System.Collections.Generic;
using System.Text;

namespace QuotingAPI.Exceptions
{
    public class ControllerExceptions : Exception
    {
        public ControllerExceptions(string message) : base(message)
        {

        }
    }
}