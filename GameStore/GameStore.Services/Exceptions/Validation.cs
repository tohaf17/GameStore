using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Services.Exceptions
{
    public static class Validation
    {
        private const string NotFoundMessage = "Object not found";
        public static void ValidateString(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{paramName} cannot be null or empty", paramName);
            }
        }

        public static void ValidateGuid(Guid id, string paramName)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"{paramName} cannot be an empty GUID", paramName);
            }
        }

        public static void ValidateNull<T>(T? entity)
        {
            if (entity is null)
            {
                throw new NotFoundException(NotFoundMessage);
            }
        }
    }
}
