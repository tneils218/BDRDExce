using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MediaController(IMediaService mediaService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(string id)
        {
            var media = await mediaService.GetByIdAsync(id);
            if (media == null)
                return NotFound();

            // Trả về file từ database
            return File(media.Content, media.ContentType, null);
        }
        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var media = await mediaService.GetByIdAsync(id);
            if (media == null)
                return NotFound();

            // Trả về file từ database
            return File(media.Content, media.ContentType, media.ContentName);
        }

    }
}