using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExamMediaController : ControllerBase
    {
        private readonly IExamMediaService _examMediaService;

        public ExamMediaController(IExamMediaService examMediaService)
        {
            _examMediaService = examMediaService;
        }
        [HttpPost]
        public async Task<ActionResult<ExamMedia>> CreateExamMedia(ExamMedia examMedia)
        {
            var result = await _examMediaService.AddAsync(examMedia);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamMedia>>> GetAllExamMedia()
        {
            var result = await _examMediaService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ExamMedia>> GetExamMediaByExamId(string mediaId, int examId)
        {
            var result = await _examMediaService.GetExamMediaById(examId, mediaId);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> DeleteExamMedia(int id)
        {
            try
            {
                await _examMediaService.DeleteAsync(id);
                return Ok("Success");
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            
        }

    }
}