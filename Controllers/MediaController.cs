using BDRDExce.Infrastructures.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(string id)
        {
            var media = await _mediaService.GetByIdAsync(id);
            if (media == null)
                return NotFound();

            // Trả về file từ database
            return File(media.Content, media.ContentType, null);
        }
        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var media = await _mediaService.GetByIdAsync(id);
            if (media == null)
                return NotFound();

            // Trả về file từ database
            return File(media.Content, media.ContentType, media.ContentName);
        }

    }
}