using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Courses.Commands;
using Domain.Courses;
using Domain.CourseUsers;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("courses")]
[ApiController]
public class CoursesController(ISender sender, ICourseQueries courseQueries, ICourseUserQueries courseUserQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await courseQueries.GetAll(cancellationToken);
        
        return entities.Select(CourseDto.FromDomainModel).ToList();
    }
    [HttpGet("GetAllCourseUser")]
    public async Task<ActionResult<IReadOnlyList<CourseUserDto>>> GetAllCourseUser(CancellationToken cancellationToken)
    {
        var entities = await courseUserQueries.GetAll(cancellationToken);
        
        return entities.Select(CourseUserDto.FromCourseUser).ToList();
    }
    [HttpPut("SetRating")]
    public async Task<ActionResult<CourseUserDto>> SetRating(Guid courseId, Guid userId, decimal rating, CancellationToken cancellationToken)
    {
        var input = new SetRatingCourseCommand
        {
            CourseId = courseId,
            UserId = userId,
            Rating = rating
        };
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            cu => CourseUserDto.FromCourseUser(cu),
            e => e.ToObjectResult());
    }
    
    [HttpGet("{userId:guid}/GetCourseByUser")]
    public async Task<ActionResult<IReadOnlyList<CourseUserDto>>> GetCourseByUser([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var entities = await courseUserQueries.GetCourseByUser(new UserId(userId), cancellationToken);
        
        return  entities.Select(CourseUserDto.FromCourseUser).ToList();
    }
    [HttpGet("{courseId:guid}/GetCourseUserByCourse")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetCourseUserByCourse([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var entities = await courseUserQueries.GetUsersByCourseId(new CourseId(courseId), cancellationToken);
        
        return  entities.Select(UserDto.FromDomainModel).ToList();
    }
    [HttpPost("CreateCourse")]
    public async Task<ActionResult<CourseDto>> Create(
        [FromBody] CourseDto request,
        CancellationToken cancellationToken)
    {
        var input = new CreateCourseCommand
        {
            Name = request.Name,
            EndDate = request.EndDate,
            StartDate = request.StartDate,
            MaxStudents = request.MaxStudents
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseDto>>(
            c => CourseDto.FromDomainModel(c),
            e => e.ToObjectResult());
    }
    [HttpPost("AddUserToCourse")]
    public async Task<ActionResult<CourseUserDto>> AddUserToCourse(
        [FromBody]
        CourseUserIdDto request,
        CancellationToken cancellationToken)
    {
        var input = new AddUserToCourseCommand
        {
            CourseId = request.CourseId,
            UserId = request.UserId 
        };
        
        var result = await sender.Send(input, cancellationToken);

        return result.Match<ActionResult<CourseUserDto>>(
            c => CourseUserDto.FromCourseUser(c),
            e => e.ToObjectResult());
    }

    [HttpPut]
    public async Task<ActionResult<CourseDto>> Update(
        [FromBody] 
        CourseDto request,
        CancellationToken cancellationToken)
    {
        var input = new UpdateCourseCommand
        {
            CourseId = request.CourseId!.Value,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            MaxStudents = request.MaxStudents
        };
            
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<CourseDto>>(
            c=>CourseDto.FromDomainModel(c),
            e=>e.ToObjectResult());
    }
    [HttpPut("FinishCourseForAll")]
    public async Task<ActionResult<IReadOnlyList<CourseUserDto>>> FinishCourseForAll(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var input = new FinishCourseForUsersCommand
        {
            CourseId = courseId
        };
            
        var result = await sender.Send(input, cancellationToken);
        
        return result.Match<ActionResult<IReadOnlyList<CourseUserDto>>>(
            c=>c.Select(CourseUserDto.FromCourseUser).ToList(),
            e=>e.ToObjectResult());
    }
}