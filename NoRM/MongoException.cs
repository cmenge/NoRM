using System;
using System.Runtime.Serialization;

namespace Norm
{
    /// <summary>
    /// Indicates an issue with some part of the messaging between C# and the MongoDB.
    /// </summary>
    [Serializable]
    public class MongoException : Exception
    {
        public MongoException() { }
        public MongoException(string message) : base(message) { }
        public MongoException(string message, Exception inner) : base(message, inner) { }
        protected MongoException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}