using BDRDExce.Infrastructures.Services.Interface;
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

        [HttpPost]
        public async Task<ActionResult> CreateSubmission(CreateSubmissionDto submissionDto)
        {
            var result = await _submissionService.AddSubmission(submissionDto, Request);
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