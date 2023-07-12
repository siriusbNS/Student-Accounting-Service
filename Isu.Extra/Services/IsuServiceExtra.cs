using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuServiceExtra : IIsuService
{
    private const int _maxOgnpGroupsInFlow = 4;
    private readonly IIsuService _isuService;
    public IsuServiceExtra()
    {
        _isuService = new IsuService();
        ListOfStudents = new List<StudentExtra>();
        ListOfGroupsExtra = new List<GroupExtra>();
        ListOfOgnpFlows = new List<OgnpFlow>();
    }

    private List<OgnpFlow> ListOfOgnpFlows { get; set; }
    private List<GroupExtra> ListOfGroupsExtra { get; set; }
    private List<StudentExtra> ListOfStudents { get; set; }
    public IReadOnlyList<GroupExtra> GetGroupsExtra() => ListOfGroupsExtra;
    public IReadOnlyList<StudentExtra> GetStudents() => ListOfStudents;
    public IReadOnlyList<OgnpGroup> GetOgnpGroups() => ListOfOgnpFlows.SelectMany(x => x.GetGroups()).ToList();
    public Group AddGroup(GroupName name)
    {
        Group group = _isuService.AddGroup(name);
        ListOfGroupsExtra.Add(new GroupExtra(group));
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        Student student = _isuService.AddStudent(group, name);
        ListOfStudents.Add(new StudentExtra(student));
        return student;
    }

    public IReadOnlyList<Group> GetGroups() => _isuService.GetGroups();

    public Student GetStudent(int id)
    {
        return _isuService.GetStudent(id);
    }

    public Student FindStudent(int id)
    {
        return _isuService.FindStudent(id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return _isuService.FindStudents(groupName);
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return _isuService.FindStudents(courseNumber);
    }

    public Group FindGroup(GroupName groupName)
    {
        return _isuService.FindGroup(groupName);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _isuService.FindGroups(courseNumber);
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        _isuService.ChangeStudentGroup(student, newGroup);
    }

    public void AddOgnpCourse(string megaFacultyName, int numberOfFlows)
    {
        if (numberOfFlows < 1)
        {
            throw new FlowNumberException("Flow number cannot be less than 1");
        }

        for (int i = 1; i <= numberOfFlows; i++)
        {
            var ognpFlow = new OgnpFlow(i, megaFacultyName);
            ListOfOgnpFlows.Add(ognpFlow);
            for (int j = 1; j <= _maxOgnpGroupsInFlow; j++)
            {
                ognpFlow.AddOgnpGroup(new OgnpGroup(megaFacultyName, j));
            }
        }
    }

    public OgnpGroup AddStudentToOgnp(string megaFacultyName, int numberOfFlow, int numberOfGroup, StudentExtra studentExtra)
    {
        if (studentExtra.MegaFacultyName == megaFacultyName)
        {
            throw new MegaFacultySameNameException("Mega faculty names are same");
        }

        if (studentExtra.OgnpCheck == 2)
        {
            throw new StudentOgnpNumberRegException("you cant have more than 2 ognp courses.");
        }

        var currentGroup = ListOfOgnpFlows
            .FirstOrDefault(x => (x.MegaFacultyName == megaFacultyName && x.FlowNumber == numberOfFlow))
            .GetGroups()
            .FirstOrDefault(group => group.NumberOfGroup == numberOfGroup);
        ArgumentNullException.ThrowIfNull(currentGroup);

        if (currentGroup.CheckLessonTime(studentExtra) is false)
        {
            throw new LessonTimeCoincidenceException("Time is engaged.");
        }

        currentGroup.AddStudent(studentExtra);
        return currentGroup;
    }

    public void RemoveStudentFromOgnpGroup(OgnpGroup ognpGroup, StudentExtra studentExtra)
    {
        if (studentExtra.OgnpCheck == 0)
        {
            throw new StudentOgnpNumberRegException("There is no student registrations in ognp");
        }

        ArgumentNullException.ThrowIfNull(ognpGroup);
        ArgumentNullException.ThrowIfNull(studentExtra);

        var currentGroup = ListOfOgnpFlows
            .SelectMany(x => x.GetGroups())
            .FirstOrDefault(currentGroup_ => currentGroup_ == ognpGroup);
        ArgumentNullException.ThrowIfNull(currentGroup);

        currentGroup.RemoveStudent(studentExtra);
    }

    public List<OgnpFlow> FindOgnpFlows(string megaFacultyName)
    {
        ArgumentNullException.ThrowIfNull(megaFacultyName);

        var currentFlows = ListOfOgnpFlows
            .Where(x => x.MegaFacultyName == megaFacultyName)
            .ToList();
        if (currentFlows.Count == 0)
        {
            throw new FlowNumberException("There are no ognp flows.");
        }

        return currentFlows;
    }

    public List<StudentExtra> FindStudentsInOgnpGroup(string megaFacultyName, int numberOfFLow, int numberOfGroup)
    {
        if ((numberOfGroup <= 0 || numberOfGroup >= _maxOgnpGroupsInFlow) || numberOfFLow <= 0)
        {
            throw new FlowNumberException("Flow or group number is uncorrect");
        }

        ArgumentNullException.ThrowIfNull(megaFacultyName);

        var currentList = ListOfOgnpFlows
            .FirstOrDefault(x => x.FlowNumber == numberOfFLow && x.MegaFacultyName == megaFacultyName)
            .GetGroups()
            .FirstOrDefault(group => group.NumberOfGroup == numberOfGroup)
            .GetStudents().ToList();
        if (currentList.Count == 0)
        {
            throw new StudentListNullException("There is no student list");
        }

        return currentList;
    }

    public List<StudentExtra> FindStudentWithoutOgnpCourse(Group group)
    {
       ArgumentNullException.ThrowIfNull(group);
       var currentList = ListOfStudents
            .Where(x => x.OgnpCheck == 0)
            .ToList();
       if (currentList.Count == 0)
       {
            throw new StudentListNullException("there is no students");
       }

       return currentList;
    }

    public void AddLessonToGroup(Lesson lesson, Group group)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        ArgumentNullException.ThrowIfNull(group);
        var currentGroup = ListOfGroupsExtra
            .FirstOrDefault(x => x.GroupOfStudents == group);
        currentGroup.AddLesson(lesson);
        var currentList = ListOfStudents
            .Where(x => x.Student.StudentGroup == group.GroupName)
            .ToList();
        foreach (var i in currentList)
        {
            i.AddLesson(lesson);
        }
    }

    public StudentExtra FindStudentExtra(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        var currentStudent = ListOfStudents
            .FirstOrDefault(x => x.Student == student);
        ArgumentNullException.ThrowIfNull(currentStudent);
        return currentStudent;
    }

    public GroupExtra FindGroupExtra(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);

        var currentGroup = ListOfGroupsExtra
            .FirstOrDefault(x => x.GroupOfStudents == group);
        ArgumentNullException.ThrowIfNull(currentGroup);
        return currentGroup;
    }

    public void AddLessonToOgnpGroup(Lesson lesson, OgnpGroup ognpGroup)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        ArgumentNullException.ThrowIfNull(ognpGroup);
        var currentGroup = ListOfOgnpFlows
            .SelectMany(x => x.GetGroups())
            .FirstOrDefault(group => group == ognpGroup);
        var currentList = ListOfOgnpFlows
            .SelectMany(x => x.GetGroups())
            .FirstOrDefault(group => group == ognpGroup)
            .GetStudents().ToList();
        ArgumentNullException.ThrowIfNull(currentGroup);
        ArgumentNullException.ThrowIfNull(currentList);
        foreach (var i in currentList)
        {
            i.AddLesson(lesson);
        }

        currentGroup.AddLesson(lesson);
    }

    public void AddGroupMegaFacultyName(Group group, string megaFacultyName)
    {
        ArgumentNullException.ThrowIfNull(megaFacultyName);
        ArgumentNullException.ThrowIfNull(group);

        var currentGroup = ListOfGroupsExtra
            .FirstOrDefault(x => x.GroupOfStudents == group);
        ArgumentNullException.ThrowIfNull(currentGroup);

        currentGroup.AddFacultyNAme(megaFacultyName);
        var currentList = ListOfStudents
            .Where(x => x.Student.StudentGroup == group.GroupName)
            .ToList();
        foreach (var i in currentList)
        {
            i.AddFacultyName(megaFacultyName);
        }
    }
}