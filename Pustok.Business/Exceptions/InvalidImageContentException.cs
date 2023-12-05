using System.Runtime.Serialization;

namespace Pustok.Business.Exceptions
{
    public class InvalidImageContentException : Exception
    {
        public string PropertyName { get; set; }
        public InvalidImageContentException()
        {
        }

        public InvalidImageContentException(string? message) : base(message)
        {
        }

        public InvalidImageContentException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
