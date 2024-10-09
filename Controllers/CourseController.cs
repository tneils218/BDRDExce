using Microsoft.AspNetCore.Mvc;
using BDRDExce.Infrastructures.Services.Interface;
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
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var file = course.Media != null ? new FileDto(course.Media.ContentName, course.Media.FileUrl) : new FileDto();
            var courseDto = new CourseDto {
                 Id = course.Id, 
                 Title = course.Title, 
                 Desc = course.Desc,
                 Label = course.Label,
                 File = file, 
                 Exams = course.Exams.Select(e => 
                 {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var filesSubmission = e.Submissions.Where(x => x.UserId == userId)
                        .SelectMany(u => u.Medias).Select(x => {return new FileDto(x.ContentName, x.FileUrl);})
                        .ToList();
                        var filesExam = e.Medias.Select(x => {return new FileDto(x.ContentName, x.FileUrl);}).ToList();
                    return new ExamDto(e.Id, e.Title, e.Content, e.CourseId, e.IsComplete, null, null);
                 }).ToList()};
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
                return Ok("Delete Course Successfully!");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}