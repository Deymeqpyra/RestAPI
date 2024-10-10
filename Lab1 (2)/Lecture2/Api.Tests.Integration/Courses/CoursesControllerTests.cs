using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Faculties;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Courses;

public class CoursesControllerTests
    :BaseIntegrationTest, IAsyncLifetime
{
    private readonly Course _mainCourse = CoursesData.MainCourse;
    private readonly Faculty _mainFaculty = FacultiesData.MainFaculty;
    private readonly Course _extraCourse = CoursesData.ExtraCourse;

    private readonly User _extraUser;
    private readonly User _mainUser;
    private readonly CourseUser _mainCoursesUser;

    public CoursesControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
        _mainUser = UsersData.MainUser(_mainFaculty.Id);
        _mainCoursesUser = CourseUsersData.MainCourseUser(_mainCourse.Id, _mainUser.Id);
        _extraUser = UsersData.ExtraUser(_mainFaculty.Id);
    }

    [Fact]
    public async Task ShouldCreateCourse()
    {
        // arrange
        var name = "Test Course";
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddYears(1);
        int maxStudent = 5;
        var request = new CourseDto(
            CourseId: null,
            Name: name,
            StartDate: startDate,
            EndDate: endDate,
            MaxStudents: maxStudent);
        
        // act
        var response = await Client.PostAsJsonAsync("courses/CreateCourse", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseCourse = await response.ToResponseModel<CourseDto>();
        var courseId = new CourseId(responseCourse.CourseId!.Value);
        
        var dbCourse = await Context.Courses.FirstAsync(x=>x.Id == courseId);
        dbCourse.Name.Should().Be(name);
        dbCourse.MaxStudents.Should().Be(maxStudent);
    }
    [Fact]
    public async Task ShouldUpdateCourse()
    {
        //arrange
        var updateName = "Test Name Update";
        var maxStudent = 7;
        var request = new CourseDto(
            CourseId: _mainCourse.Id.Value,
            Name: updateName,
            StartDate: _mainCourse.StartDate,
            EndDate: _mainCourse.EndDate,
            MaxStudents: maxStudent);
        
        // act
        var response = await Client.PutAsJsonAsync("courses", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var courseFromResponse = await response.ToResponseModel<CourseDto>();
        
        var courseFromDataBase = await Context.Courses
            .FirstAsync(x=>x.Id == new CourseId(courseFromResponse.CourseId!.Value));
        
        courseFromDataBase.Should().NotBeNull();
        courseFromDataBase.Name.Should().Be(updateName);
        courseFromDataBase.MaxStudents.Should().Be(maxStudent);
    }

    [Fact]
    public async Task ShouldNotCreateBecauseNameDuplicated()
    {
        // arrange
        var request = new CourseDto(
            CourseId: null,
            Name: _mainCourse.Name,
            StartDate: _mainCourse.StartDate,
            EndDate: _mainCourse.EndDate,
            MaxStudents: _mainCourse.MaxStudents);
        
        // act
        var response = await Client.PostAsJsonAsync("courses/CreateCourse", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateBecauseCourseNotFound()
    {
        //arrange 
        var request = new CourseDto(
            CourseId: Guid.NewGuid(), 
            Name: _mainCourse.Name,
            StartDate: _mainCourse.StartDate,
            EndDate: _mainCourse.EndDate,
            MaxStudents: _mainCourse.MaxStudents);
        
        //act
        var response = await Client.PutAsJsonAsync("courses", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldNotCreateBecauseMaxStudentsIsNotValid()
    {
        //arrange 
        var maxStudentsNotValid = -1;
        var request = new CourseDto(
            CourseId: Guid.NewGuid(), 
            Name: _mainCourse.Name,
            StartDate: _mainCourse.StartDate,
            EndDate: _mainCourse.EndDate,
            MaxStudents:maxStudentsNotValid );
        
        //act
        var response = await Client.PostAsJsonAsync("courses/CreateCourse", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldCreateCourseUser()
    {
        // arrange 
        var request = new CourseUserDto(
            CourseUserId: null,
            CourseId: _extraCourse.Id.Value,
            Course: null,
            UserId: _extraUser.Id.Value,
            User: null,
            Rating: null,
            IsFinished: null);
        
        // act 
        var response = await Client.PostAsJsonAsync("courses/AddUserToCourse", request);
        
        // assert 
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotCreateCourseUserBecauseCourseNotFound()
    {
        // arrange 
        var request = new CourseUserDto(
            CourseUserId: null,
            CourseId: Guid.NewGuid(), 
            Course: null,
            UserId: _extraUser.Id.Value, 
            User: null,
            Rating: null,
            IsFinished: null);
        
        // act
        var response = await Client.PostAsJsonAsync("courses/AddUserToCourse", request);
        
        // assert 
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);        
    }
    
    [Fact]
    public async Task ShouldNotCreateCourseUserBecauseUserNotFound()
    {
        // arrange 
        var request = new CourseUserDto(
            CourseUserId: null,
            CourseId: _extraCourse.Id.Value,
            Course: null,
            UserId: Guid.NewGuid(), 
            User: null,
            Rating: null,
            IsFinished: null);
        
        // act
        var response = await Client.PostAsJsonAsync("courses/AddUserToCourse", request);
        
        // assert 
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);        
    }
    
    public async Task InitializeAsync()
    {
        await Context.Faculties.AddAsync(_mainFaculty);
        await Context.Courses.AddAsync(_mainCourse);
        await Context.Courses.AddAsync(_extraCourse);
        await Context.Users.AddAsync(_extraUser);
        await Context.Users.AddAsync(_mainUser);
        await Context.CourseUsers.AddAsync(_mainCoursesUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Courses.RemoveRange(Context.Courses);
        Context.Faculties.RemoveRange(Context.Faculties);
        Context.Users.RemoveRange(Context.Users);
        Context.CourseUsers.RemoveRange(Context.CourseUsers);
        await SaveChangesAsync();
    }
}