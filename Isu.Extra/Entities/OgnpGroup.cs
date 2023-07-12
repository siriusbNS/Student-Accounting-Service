using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class OgnpGroup
{
    private const short StudentsMaximum = 4;
    private const short StudentsMinimum = 0;
    public OgnpGroup(string megaFacultyName, int ognpNumber)
    {
        ArgumentNullException.ThrowIfNull(megaFacultyName);
        ArgumentNullException.ThrowIfNull(ognpNumber);
        MegaFacultyName = megaFacultyName;
        NumberOfGroup = ognpNumber;
        ListOfStudents = new List<StudentExtra>();
        ListOfLessons = new List<Lesson>();
    }

    public short NumberOfStudents { get; set; } = 0;
    public string MegaFacultyName { get; private set; }
    public int NumberOfGroup { get; private set; }
    public int FlowNumber { get; set; }
    private List<StudentExtra> ListOfStudents { get; set; }
    private List<Lesson> ListOfLessons { get; set; }
    public IReadOnlyList<StudentExtra> GetStudents() => ListOfStudents;
    public IReadOnlyList<Lesson> GetLessons() => ListOfLessons;

    public bool CheckLessonTime(StudentExtra studentExtra)
    {
        var currentLesson = ListOfLessons
            .FirstOrDefault(x => studentExtra.FindLesson(x.LessonDate, x.LessonStartTime));
        return currentLesson is null;
    }

    public void CheckNumberOfStudents()
    {
        if (NumberOfStudents is > StudentsMaximum or < StudentsMinimum)
            throw new GroupException("Unrealiable number of students.");
    }

    public void AddStudent(StudentExtra studentExtra)
    {
        ArgumentNullException.ThrowIfNull(studentExtra);

        ListOfStudents.Add(studentExtra);
        NumberOfStudents++;
        CheckNumberOfStudents();

        ListOfLessons.ForEach(studentExtra.AddLesson);

        studentExtra.OgnpCheckButton(true);
    }

    public void RemoveStudent(StudentExtra studentExtra)
    {
        ArgumentNullException.ThrowIfNull(studentExtra);

        ListOfStudents.Remove(studentExtra);
        ListOfLessons.ForEach(studentExtra.RemoveLesson);

        NumberOfStudents--;
        studentExtra.OgnpCheckButton(false);
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);
        var currentLesson = ListOfLessons
            .FirstOrDefault(x => x.LessonStartTime == lesson.LessonStartTime && x.LessonDate == lesson.LessonDate);
        if (currentLesson is not null) throw new LessonTimeCoincidenceException("This time is engaged.");
        ListOfLessons.Add(lesson);
    }
}