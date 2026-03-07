using System;
using System.Collections.Generic;
using System.Text;

namespace GameStoreApi.Validation
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message) {}
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation failures have occurred.") 
        {
            Errors = errors;
        }
    }

}
