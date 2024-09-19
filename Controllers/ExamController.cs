using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ExamController : ControllerBase
{
    private readonly IExamService _examService;
    public ExamController(IExamService examService)
    {
        _examService = examService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExam()
    {
        var result = await _examService.GetAllExam();
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ExamDto>> AddExam(CreateExamDto createExamDto)
    {
        var result = await _examService.AddExam(createExamDto, Request);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("courseId")]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetExamByCourseId(int courseId)
    {
        var result = await _examService.GetExamsByCourseId(courseId);
        return Ok(result);
    }
}