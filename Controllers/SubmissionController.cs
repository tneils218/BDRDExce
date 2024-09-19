using System.Runtime.CompilerServices;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateSubmission(CreateSubmissionDto submissionDto)
        {
            var submission = new Submission
            {
                Content = submissionDto.Content,
                CourseId = submissionDto.ExamId,
                UserId = submissionDto.UserId
            };
            var result = await _submissionService.AddAsync(submission);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteSubmission(int id)
        {
            try
            {
                await _submissionService.DeleteAsync(id);
                return Ok("Delete success");
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetAllSubmission()
        {
            var result = await _submissionService.GetAllAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("id")]
        public async Task<ActionResult<SubmissionDto>> GetSubmissionById(int id)
        {
            var result = await _submissionService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}