using Microsoft.AspNetCore.Mvc;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private const long _maxFileSize = 5 * 1024 * 1024;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExams()
        {
            var exams = await _examService.GetAllAsync();
            var examDto = exams.Select(x => {
                return new ExamDto{Id = x.Id, Content = x.Content, Title = x.Title, Label = x.Label};
            });
            return Ok(examDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetExamById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            var examDto = new ExamDto{Content = exam.Content, Title = exam.Title, Label = exam.Label};
            return Ok(examDto);
        }

        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam(CreateExamDto examDto)
        {
            try
            {
                var result = await _examService.AddExam(examDto, Request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam(int id, ChangeExamDto examDto)
        {
            if (id != examDto.Id)
            {
                return BadRequest();
            }

            try
            {
                var exam = new Exam {
                    Id = examDto.Id,
                    Title = examDto.Title,
                    Content = examDto.Content,
                    Label = examDto.Label
                };
                var updatedExam = await _examService.UpdateAsync(exam);
                return Ok(updatedExam);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            try
            {
                await _examService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
