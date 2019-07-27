using System;

namespace Weatherman.Domain.Exceptions
{
    public class MissingUserLocationException : Exception
    {
        public MissingUserLocationException()
            : base("A location was not provided or no previous locations have been used")
        {
        }
    }
}
