using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Business.Exceptions
{
    public class InvalidImageSizeException : Exception
    {
        public string PropertyName { get; }
        public InvalidImageSizeException(string prop, string msg) : base(msg)
        {
            this.PropertyName = prop;
        }
    }
}
