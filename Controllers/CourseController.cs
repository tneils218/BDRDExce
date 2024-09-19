using Microsoft.AspNetCore.Mvc;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllAsync();
            var courseDto = courses.Select(x => {
                var fileMedias = x.Medias.ToList();
                var files = fileMedias.Select(f => 
                {
                    return new Models.File {Name = f.ContentName, Url = f.FileUrl};
                }).ToList();
                return new CourseDto{Id = x.Id, Title = x.Title, Desc = x.Desc,Files = files , Label = x.Label};
            });
            return Ok(courseDto);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDto = new CourseDto{Id = course.Id, Title = course.Title, Desc = course.Desc, Label = course.Label};
            return Ok(courseDto);
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
