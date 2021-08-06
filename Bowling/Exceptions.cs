using System;
namespace Bowling
{
    public class InvalidRollException : Exception
    {
        public InvalidRollException() { }

        public InvalidRollException(string message)
            : base($"Invalid Roll: {message}") { }
    }

    public class NumPinsException : Exception
    {
        public NumPinsException() { }

        public NumPinsException(string message)
            : base($"{message}") { }
    }

    public class DuplicatePlayerException: Exception
    {
        public DuplicatePlayerException()
            : base($"Cannot add players with duplicate names") { }
    }
}
