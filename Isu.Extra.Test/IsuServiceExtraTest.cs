using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Extra.Tools;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuServiceExtra _isuService = new IsuServiceExtra();
    [Fact]
    public void AddMoreThanTwoOgnpCourses_ThrowException()
    {
        Group myGroup = _isuService.AddGroup(new GroupName("M3208"));
        Student student = _isuService.AddStudent(myGroup, "vlad ker");
        _isuService.AddOgnpCourse("ITIP", 3);
        _isuService.AddOgnpCourse("KIB", 3);
        _isuService.AddOgnpCourse("KT", 4);
        StudentExtra studentExtra = _isuService.FindStudentExtra(student);
        GroupExtra groupExtra = _isuService.FindGroupExtra(myGroup);
        OgnpGroup ognpGroup = _isuService.AddStudentToOgnp("ITIP", 2, 2, studentExtra);
        _isuService.AddStudentToOgnp("KIB", 1, 3, studentExtra);
        Assert.Throws<StudentOgnpNumberRegException>(() => _isuService.AddStudentToOgnp("KT", 2, 2, studentExtra));
    }

    [Fact]
    public void AddLessonsOfOgnpAndOtherAtTheSameTime_ThrowException()
    {
        Group myGroup = _isuService.AddGroup(new GroupName("M3208"));
        Student student = _isuService.AddStudent(myGroup, "vlad ker");
        Lesson lessonFirst = new Lesson(new TimeOnly(15, 30), new DateOnly(2021, 10, 15), 280, "Grisha", "OOp");
        Lesson lessonTwo = new Lesson(new TimeOnly(15, 30), new DateOnly(2021, 10, 15), 282, "Roma", "KIBERBEZ");

        _isuService.AddOgnpCourse("ITIP", 3);
        StudentExtra studentExtra = _isuService.FindStudentExtra(student);
        GroupExtra groupExtra = _isuService.FindGroupExtra(myGroup);
        OgnpGroup ognpGroup = _isuService.AddStudentToOgnp("ITIP", 2, 2, studentExtra);
        _isuService.AddLessonToGroup(lessonFirst, myGroup);
        Assert.Throws<LessonTimeCoincidenceException>(() => _isuService.AddLessonToOgnpGroup(lessonTwo, ognpGroup));
    }

    [Fact]
    public void AddStudentToOgnpWithHisMegaFaculty_ThrowException()
    {
        Group myGroup = _isuService.AddGroup(new GroupName("M3208"));
        Student student = _isuService.AddStudent(myGroup, "vlad ker");
        _isuService.AddGroupMegaFacultyName(myGroup, "ITIP");
        _isuService.AddOgnpCourse("ITIP", 3);
        StudentExtra studentExtra = _isuService.FindStudentExtra(student);
        Assert.Throws<MegaFacultySameNameException>(() => _isuService.AddStudentToOgnp("ITIP", 2, 2, studentExtra));
    }
}