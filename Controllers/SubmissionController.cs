using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionController(ISubmissionService submissionService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateSubmission(CreateSubmissionDto submissionDto)
        {
            var result = await submissionService.AddSubmission(submissionDto, Request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteSubmission(int id)
        {
            try
            {
                await submissionService.DeleteAsync(id);
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
            var result = await submissionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<ActionResult<SubmissionDto>> GetSubmissionById(int id)
        {
            var result = await submissionService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}