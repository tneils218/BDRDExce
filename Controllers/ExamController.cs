using System.Security.Claims;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ExamController(IExamService examService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExam()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await examService.GetAllExam(userId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExamDto>> AddExam(CreateExamDto createExamDto)
    {
        var result = await examService.AddExam(createExamDto, Request);
        return Ok(result);
    }

    [HttpGet("courseId")]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetExamByCourseId(int courseId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await examService.GetExamsByCourseId(courseId, userId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExam(int id)
    {
        await examService.DeleteAsync(id);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> EditExam(ChangeExamDto examDto)
    {
        var exam = new Exam {
            Id = examDto.id,
            Content = examDto.Content,
            Title = examDto.Title,
            CourseId = examDto.CourseId
        };
        var result = await examService.UpdateAsync(exam);
        return Ok(result);
    }
}