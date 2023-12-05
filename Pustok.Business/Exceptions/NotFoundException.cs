using System.Runtime.Serialization;

namespace Pustok.Business.Exceptions
{
    public class NotFoundException : Exception
    {
        public string PropertyName { get; set; }
        public NotFoundException()
        {
        }

        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(string propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
