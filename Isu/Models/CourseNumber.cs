using Isu.Exceptions;

namespace Isu.Models;
public class CourseNumber
{
    private const int CourseNumberMax = 4;
    private const int CourseNumberMin = 0;
    public CourseNumber(int num)
    {
        if (num is > CourseNumberMax || num is <= CourseNumberMin)
            throw new CourseNumberException("This course number does not exist");
        CourseNum = num;
    }

    public int CourseNum { get; private set; }
}