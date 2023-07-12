using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Extra.Models;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class StudentExtra
{
    private static readonly Regex FacultyNameRegex = new (@"^\w*", RegexOptions.Compiled);
    public StudentExtra(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        ListOfLessons = new List<Lesson>();
        MegaFacultyName = string.Empty;
        Student = student;
        OgnpCheck = 0;
    }

    public int OgnpCheck { get; private set; }
    public string MegaFacultyName { get; set; }
    public Student Student { get; private set; }
    private List<Lesson> ListOfLessons { get; set; }
    public IReadOnlyList<Lesson> GetLessons() => ListOfLessons;

    public bool FindLesson(DateOnly date, TimeOnly time)
    {
        var currentLesson = ListOfLessons
            .FirstOrDefault(x => x.LessonStartTime == time && x.LessonDate == date);
        return !(currentLesson is null);
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        if (FindLesson(lesson.LessonDate, lesson.LessonStartTime) is true)
        {
            throw new LessonTimeCoincidenceException("This time is engaged");
        }

        ListOfLessons.Add(lesson);
    }

    public void RemoveLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        ListOfLessons.Remove(lesson);
    }

    public void OgnpCheckButton(bool check)
    {
        if (check)
            OgnpCheck++;
        if (!check)
            OgnpCheck--;
    }

    public void AddFacultyName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        MegaFacultyName = name;
    }
}