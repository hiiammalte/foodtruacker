using System;

namespace foodtruacker.QueryRepository.Repository
{
    class MongoDbException : Exception
    {
        public MongoDbException() { }
        public MongoDbException(string message) : base(message) { }
        public MongoDbException(string message, Exception inner) : base(message, inner) { }
    }
}
