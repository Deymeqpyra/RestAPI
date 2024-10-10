using System.Configuration;
using System.Net;
using System.Net.Http.Json;
using Api.Controllers;
using Api.Dtos;
using Domain.Faculties;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Users;

public class UsersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Faculty _mainFaculty = FacultiesData.MainFaculty;
    private readonly User _mainUser;
    
    public UsersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainUser = UsersData.MainUser(_mainFaculty.Id);
    }
    
    [Fact]
    public async Task ShouldCreateUser()
    {
        // Arrange
        var firstName = "firstName";
        var lastName = "lastName";
        var request = new UserDto(
            Id: null,
            FirstName: firstName,
            LastName: lastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);
        
        // Act
        var response = await Client.PostAsJsonAsync("users", request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseUser = await response.ToResponseModel<UserDto>();
        var userId = new UserId(responseUser.Id!.Value);
        
        var dbUser = await Context.Users.FirstAsync(x=>x.Id == userId);
        dbUser.FirstName.Should().Be(firstName);
        dbUser.LastName.Should().Be(lastName);
        dbUser.FullName.Should().NotBeEmpty();
        dbUser.FacultyId.Value.Should().Be(_mainFaculty.Id.Value);
    }

    [Fact]
    public async Task ShouldUpdateUser()
    {
        // Arrange
        var newFirstName = "firstNameUpdate";
        var newLastName = "lastNameUpdate";
        var request = new UserDto(
            Id: _mainUser.Id.Value,
            FirstName: newFirstName,
            LastName: newLastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);
        // Act
        var response = await Client.PutAsJsonAsync("users", request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var userFromResponse = await response.ToResponseModel<UserDto>();

        var userFromDataBase = await Context.Users
            .FirstOrDefaultAsync(x => x.Id == new UserId(userFromResponse.Id!.Value));

        userFromDataBase.Should().NotBeNull();
        userFromDataBase.FirstName.Should().Be(newFirstName);
    }

    [Fact]
    public async Task ShouldNotCreateUserBecauseNullName()
    {
        //Arrange
        var request = new UserDto(
            Id: null,
            FirstName: null,
            LastName: _mainUser.LastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);
        
        // act
        var response = await Client.PostAsJsonAsync("users", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task ShouldNotCreateUserBecauseDuplicateName()
    {
        //Arrange
        var request = new UserDto(
            Id: null,
            FirstName: _mainUser.FirstName,
            LastName: _mainUser.LastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);
        
        // act
        var response = await Client.PostAsJsonAsync("users", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotUpdateUserBecauseUserNotFound()
    {
        // arrange 
        var newFirstName = "firstNameUpdate";
        var newLastName = "lastNameUpdate";
        var request = new UserDto(
            Id: Guid.NewGuid(),
            FirstName: newFirstName,
            LastName: newLastName,
            FullName: null,
            UpdatedAt: null,
            FacultyId: _mainFaculty.Id.Value,
            Faculty: null);
        
        //act 
        var response = await Client.PutAsJsonAsync("users", request);
        
        // assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    public async Task InitializeAsync()
    {
        await Context.Faculties.AddAsync(_mainFaculty);
        await Context.Users.AddAsync(_mainUser);
        
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Users.RemoveRange(Context.Users);
        
        await SaveChangesAsync();
    }
}