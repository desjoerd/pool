using System;
using System.Runtime.Serialization;

namespace DePool
{
    [Serializable]
    public class DePoolException : Exception
    {
        public DePoolException()
        {
        }

        public DePoolException(string message) : base(message)
        {
        }

        protected DePoolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DePoolException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
