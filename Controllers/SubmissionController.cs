using System.Runtime.CompilerServices;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        public async Task<ActionResult> CreateSubmission(CreateSubmissionDto submissionDto)
        {
            var submission = new Submission
            {
                ExamId = submissionDto.ExamId,
                UserId = submissionDto.UserId
            };
            if(submissionDto.Content != null)
            {
                submission.Content = submissionDto.Content;
            }
            if(submissionDto.File != null)
            {
                using(var ms = new MemoryStream())
                {
                    await submissionDto.File.CopyToAsync(ms);
                    var fileToBase64 = Convert.ToBase64String(ms.ToArray());
                    submission.Content = fileToBase64;
                }
            }
            var result = await _submissionService.AddAsync(submission);
            return Ok(result);
        }

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubmissionDto>>> GetAllSubmission()
        {
            var result = await _submissionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<ActionResult<SubmissionDto>> GetSubmissionById(int id)
        {
            var result = await _submissionService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}