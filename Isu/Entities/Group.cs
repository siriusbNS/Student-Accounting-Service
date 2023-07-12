using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;
public class Group
{
    private const short StudentsMaximum = 4;
    private const short StudentsMinimum = 0;
    public Group(GroupName nameNameOfGroup)
    {
        if (nameNameOfGroup == null)
            throw new GroupNameNullException("Name of group is nullable");
        GroupName = nameNameOfGroup;
        CourseNumber = new CourseNumber(1);
        ListOfStudents = new List<Student>(0);
    }

    public GroupName GroupName { get; private set; }
    public CourseNumber CourseNumber { get; private set; }
    public short NumberOfStudents { get; set; } = 0;
    private List<Student> ListOfStudents { get; set; }
    public void CheckNumberOfStudents()
    {
        if (NumberOfStudents is > StudentsMaximum or < StudentsMinimum)
            throw new GroupException("Unrealiable number of students.");
    }

    public void AddStudent(Student student)
    {
        if (student == null)
            throw new StudentNullException("Student is nullable.");
        NumberOfStudents++;
        CheckNumberOfStudents();
        ListOfStudents.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        if (student == null)
            throw new StudentNullException("Student is nullable.");
        NumberOfStudents--;
        ListOfStudents.Remove(student);
    }

    public IReadOnlyList<Student> GetStudents() => ListOfStudents;
}