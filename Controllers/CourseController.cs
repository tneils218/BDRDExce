using Microsoft.AspNetCore.Mvc;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace BDRDExce.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(string userId)
        {
            if(userId == null)
            {
                var courses = await _courseService.GetAllAsync();
                var courseDto = courses.Select(x => {
                    return new CourseDto{Id = x.Id, Title = x.Title, Desc = x.Desc, Label = x.Label, Exams = x.Exams.Select(e => new ExamDto(e.Title, e.Content, e.CourseId, e.IsComplete)).ToList()};
                });
                return Ok(courseDto);
            }
            else
            {
                var courses = await _courseService.GetCoursesByUserIdAsync(userId);
                var courseDto = courses.Select(x => {
                    return new CourseDto{Id = x.Id, Title = x.Title, Desc = x.Desc, Label = x.Label, Exams = x.Exams.Select(e => new ExamDto(e.Title, e.Content, e.CourseId, e.IsComplete)).ToList()};
                });
                return Ok(courseDto);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDto = new CourseDto{Id = course.Id, Title = course.Title, Desc = course.Desc, Label = course.Label, Exams = course.Exams.Select(e => new ExamDto(e.Title, e.Content, e.CourseId, e.IsComplete)).ToList()};
            return Ok(courseDto);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto courseDto)
        {
            try
            {
                var result = await _courseService.AddCourse(courseDto, Request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, ChangeCourseDto courseDto)
        {
            if (id != courseDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var course = new Course {
                    Id = courseDto.Id,
                    Title = courseDto.Title,
                    Label = courseDto.Label
                };
                var updatedCourse = await _courseService.UpdateAsync(course);
                return Ok(updatedCourse);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
