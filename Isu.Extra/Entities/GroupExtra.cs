using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Extra.Models;
using Isu.Extra.Tools;
using Group = Isu.Entities.Group;

namespace Isu.Extra.Entities;

public class GroupExtra
{
    public GroupExtra(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        GroupOfStudents = group;
        MegaFacultyName = string.Empty;
        ListOfLessons = new List<Lesson>();
    }

    public Group GroupOfStudents { get; private set; }
    public string MegaFacultyName { get; private set; }
    private List<Lesson> ListOfLessons { get; set; }
    public IReadOnlyList<Lesson> GetLessons() => ListOfLessons;

    public void AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        GroupOfStudents.AddStudent(student);
    }

    public void RemoveStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        GroupOfStudents.RemoveStudent(student);
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        var currentLesson = ListOfLessons
            .FirstOrDefault(x => x.LessonStartTime == lesson.LessonStartTime && x.LessonDate == lesson.LessonDate);
        if (currentLesson is null)
        {
            ListOfLessons.Add(lesson);
            return;
        }

        throw new LessonTimeCoincidenceException("This time is engaged.");
    }

    public void AddFacultyNAme(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        MegaFacultyName = name;
    }
}