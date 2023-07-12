using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IIsuService _isuService = new IsuService();
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        Group myGroup = _isuService.AddGroup(new GroupName("M3208"));
        Student student = _isuService.AddStudent(myGroup, "vlad ker");
        Assert.Contains(student, myGroup.GetStudents());
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        Group myGroup = _isuService.AddGroup(new GroupName("M3208"));
        Student studentFirst = _isuService.AddStudent(myGroup, "vlad kers");
        Student studentSecond = _isuService.AddStudent(myGroup, "vlad kerl");
        Student studentThird = _isuService.AddStudent(myGroup, "vlad kerg");
        Student studentFourth = _isuService.AddStudent(myGroup, "vlad kert");
        Assert.Throws<GroupException>(() => _isuService.AddStudent(myGroup, "vlad kerz"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<GroupNameException>(() => new GroupName("Invalid name"));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        Group myGroupFirst = _isuService.AddGroup(new GroupName("M3208"));
        Group myGroupSecond = _isuService.AddGroup(new GroupName("M3207"));
        Student studentFirst = _isuService.AddStudent(myGroupFirst, "vlad kerg");
        Student studentSecond = _isuService.AddStudent(myGroupFirst, "vlad kerl");
        _isuService.ChangeStudentGroup(studentFirst, myGroupSecond);
        Assert.Contains(studentFirst, myGroupSecond.GetStudents());
    }

    [Theory]
    [InlineData("M3208")]
    [InlineData("N45021")]
    public void AddGroupToList_FindGroupInList(string groupName)
    {
        GroupName newGroupName = new GroupName(groupName);
        Group newGroup = _isuService.AddGroup(newGroupName);
        Group secondGroup = _isuService.FindGroup(newGroupName);
        Assert.Contains(secondGroup, _isuService.GetGroups());
    }
}