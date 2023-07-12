using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private const int NullId = 0;
    private int id = 1;
    public IsuService()
    {
        GroupsList = new List<Group>();
    }

    private List<Group> GroupsList { get; set; }
    public IReadOnlyList<Group> GetGroups() => GroupsList;
    public Group AddGroup(GroupName name)
    {
        if (name is null)
        {
            throw new GroupNameNullException("Group name is nullable.");
        }

        Group group = new Group(name);
        GroupsList.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        Student student = new Student(name, group.GroupName, group.CourseNumber, id, "nothing");
        var currentGroup = GroupsList.FirstOrDefault(x => x == group);
        if (group is null)
        {
            throw new GroupNullException("There is no this group.");
        }

        if (currentGroup is null)
        {
            throw new GroupNullException("There is no this group.");
        }

        id++;
        currentGroup.AddStudent(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        if (id <= NullId)
        {
            throw new IdNullException("Id is nullable.");
        }

        var student = GroupsList
            .SelectMany(x => x.GetStudents())
            .FirstOrDefault(student => student.StudentId == id);

        if (student is null) throw new StudentException("Student can not be got.");
        return student;
    }

    public Student? FindStudent(int id)
    {
        if (id <= NullId)
        {
            throw new IdNullException("Id is nullable.");
        }

        var student = GroupsList
            .SelectMany(x => x.GetStudents())
            .FirstOrDefault(student => student.StudentId == id);
        return student;
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        if (groupName is null)
        {
            throw new GroupNameNullException("Group name is nullable");
        }

        return GroupsList
            .SelectMany(x => x.GetStudents())
            .Where(student => student.StudentGroup == groupName)
            .ToList();
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        if (courseNumber is null)
        {
            throw new CourseNumberNullException("Course number is nullable.");
        }

        return GroupsList
            .SelectMany(x => x.GetStudents())
            .Where(student => student.StudentCourse == courseNumber)
            .ToList();
    }

    public Group? FindGroup(GroupName groupName)
    {
        if (groupName is null)
        {
            throw new GroupNameNullException("Group name is nullable");
        }

        return GroupsList
            .FirstOrDefault(group => group.GroupName == groupName);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        if (courseNumber is null)
        {
            throw new CourseNumberNullException("Course number is nullable.");
        }

        var groups = GroupsList
            .FindAll(groups => groups.CourseNumber == courseNumber);
        if (groups.Capacity == 0)
            throw new GroupException("List of Groups is null.");
        return groups;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (student is null)
        {
            throw new StudentNullException("Student is nullable.");
        }

        if (newGroup is null)
        {
            throw new GroupNullException("Group is nullable.");
        }

        GroupName oldGroupName = student.StudentGroup;
        student.StudentGroup = newGroup.GroupName;
        var newStudentGroup = GroupsList
            .FirstOrDefault(newStudentGroup => newStudentGroup == newGroup);
        if (newStudentGroup is null)
        {
            throw new GroupNullException("Group is nullable.");
        }

        newStudentGroup.AddStudent(student);
        newStudentGroup.CheckNumberOfStudents();
        var oldStudentGroup = GroupsList
            .FirstOrDefault(oldStudentGroup => oldStudentGroup.GroupName == oldGroupName);
        if (oldStudentGroup is null)
        {
            throw new GroupNullException("Group is nullable.");
        }

        oldStudentGroup.RemoveStudent(student);
    }
}