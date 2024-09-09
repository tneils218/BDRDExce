using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SubmissionMediaController : ControllerBase
    {
        private readonly ISubmissionMediaService _submissionMediaservice;
        public SubmissionMediaController(ISubmissionMediaService submissionMediaService)
        {
            _submissionMediaservice = submissionMediaService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubmissionMedia>>> GetAllSubmissionMedia()
        {
            var result = await _submissionMediaservice.GetAllAsync();
            return Ok(result);
        }
    }
}