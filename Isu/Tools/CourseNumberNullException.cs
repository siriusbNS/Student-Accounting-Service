namespace Isu.Exceptions;

internal class CourseNumberNullException : Exception
{
    public CourseNumberNullException(string message)
        : base(message) { }
}