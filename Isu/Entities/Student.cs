using System.Text.RegularExpressions;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Student
{
    private static readonly Regex StudentNameRegex = new (@"^\w*\s\w*$", RegexOptions.Compiled);

    public Student(string studentName, GroupName studentGroup, CourseNumber studentCourse, int studentId, string studentFaculty)
    {
        CheckStudentNameAndFaculty(studentName);
        ProcessingDataOfNull(studentName, studentGroup, studentCourse, studentId);
        StudentName = studentName;
        StudentId = studentId;
        StudentCourse = studentCourse;
        StudentGroup = studentGroup;
        StudentFaculty = studentFaculty;
    }

    public string StudentName { get; private set; }
    public GroupName StudentGroup { get;  set; }
    public CourseNumber StudentCourse { get; private set; }
    public string StudentFaculty { get; private set; }
    public int StudentId { get; }
    public void ChangeFaculty(string newFaculty)
    {
        StudentFaculty = newFaculty;
    }

    private void CheckStudentNameAndFaculty(string studentName)
    {
        if (!StudentNameRegex.IsMatch(studentName))
        {
            throw new StudentException("Uncorrect name of student.");
        }
    }

    private void ProcessingDataOfNull(string studentName, GroupName studentGroup, CourseNumber studentCourse, int studentId)
    {
        if (studentId <= 0)
        {
            throw new IdNullException("Id is nullable.");
        }

        if (studentGroup == null)
            throw new GroupNameNullException("Name of group is nullable");
        if (studentCourse == null)
            throw new CourseNumberNullException("Course number of group is nullable");
        if (studentName == null)
            throw new GroupNameNullException("Name of student is nullable");
    }
}