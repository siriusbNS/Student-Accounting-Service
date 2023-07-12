using System.Text.RegularExpressions;
using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Models;

public class Lesson
{
    private static readonly Regex MentorNameRegex = new (@"^\w*", RegexOptions.Compiled);
    public Lesson(TimeOnly lessonStartTime, DateOnly dateOnly, int audienceNumber, string mentorName, string nameOfLesson)
    {
        if (!MentorNameRegex.IsMatch(mentorName))
        {
            throw new MentorNameException("Uncorrect name of mentor.");
        }

        NameOfLesson = nameOfLesson;
        LessonStartTime = lessonStartTime;
        LessonDate = dateOnly;
        AudienceNumber = audienceNumber;
        MentorName = mentorName;
    }

    public string NameOfLesson { get; private set; }
    public TimeOnly LessonStartTime { get; private set; }
    public DateOnly LessonDate { get; private set; }
    public int AudienceNumber { get; private set; }
    public string MentorName { get; private set; }
}