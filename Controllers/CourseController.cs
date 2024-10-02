using Microsoft.AspNetCore.Mvc;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BDRDExce.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CourseController(ICourseService courseService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses(string userId)
        {
            var idUser = userId == null ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : userId;
            var courses = await courseService.GetCoursesByUserIdAsync(idUser);
            var courseDto = courses.Select(c => 
            {
                return new CourseDto { Id = c.Id, Title = c.Title, Desc = c.Desc, Label = c.Label, ImageUrl = c.ImageUrl, Exams = c.Exams.Select(e => new ExamDto(e.Id, e.Title, e.Content, e.CourseId, e.IsComplete)).ToList() };
            });
            return Ok(courseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseDto = new CourseDto { Id = course.Id, Title = course.Title, Desc = course.Desc, Label = course.Label, Exams = course.Exams.Select(e => new ExamDto(e.Id, e.Title, e.Content, e.CourseId, e.IsComplete)).ToList() };
            return Ok(courseDto);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CreateCourseDto courseDto)
        {
            try
            {
                var result = await courseService.AddCourse(courseDto, Request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(ChangeCourseDto courseDto)
        {
            try
            {
                var updatedCourse = await courseService.UpdateCourseAsync(courseDto, Request);
                if(updatedCourse == null)
                    return BadRequest("Course Not Found!");
                return Ok(updatedCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await courseService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}