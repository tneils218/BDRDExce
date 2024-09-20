using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ExamController : ControllerBase
{
    private readonly IExamService _examService;
    public ExamController(IExamService examService)
    {
        _examService = examService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExam()
    {
        var result = await _examService.GetAllExam();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExamDto>> AddExam(CreateExamDto createExamDto)
    {
        var result = await _examService.AddExam(createExamDto, Request);
        return Ok(result);
    }

    [HttpGet("courseId")]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetExamByCourseId(int courseId)
    {
        var result = await _examService.GetExamsByCourseId(courseId);
        return Ok(result);
    }
}