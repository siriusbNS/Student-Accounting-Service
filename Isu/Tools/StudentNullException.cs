namespace Isu.Exceptions;

internal class StudentNullException : Exception
{
    public StudentNullException(string message)
        : base(message) { }
}