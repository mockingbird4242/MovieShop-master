using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.Exceptions
{
    public class MovieConflictException :Exception
    {
        public MovieConflictException(string message) : base(message)
        {

        }
    }
}
