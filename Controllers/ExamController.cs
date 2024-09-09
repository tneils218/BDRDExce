using Microsoft.AspNetCore.Mvc;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using System.Reflection.Metadata.Ecma335;

namespace BDRDExce.Controllers
{
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
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExams()
        {
            var exams = await _examService.GetAllAsync();
            var examDto = exams.Select(x => {
                return new ExamDto{Content = x.Content, UserId = x.UserId, Medias = x.Medias};
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
            return Ok(exam);
        }

        [HttpPost]
        public async Task<ActionResult<CreateExamDto>> CreateExam([FromForm] CreateExamDto examDto)
        {
            var medias = new List<Media>();
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Kiểm tra xem thư mục upload có tồn tại không, nếu không thì tạo
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Duyệt qua danh sách các file đã upload
            foreach (var file in examDto.Files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Tạo URL để truy cập file đã lưu
                    var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                    // Tạo đối tượng Media từ tệp tin
                    var media = new Media
                    {
                        Id = Guid.NewGuid().ToString(),
                        ContentType = file.ContentType,
                        ContentName = file.FileName,
                        Content = null,  
                        FileUrl = fileUrl   
                    };

                    medias.Add(media);
                }
            }

            // Tạo đối tượng Exam
            var exam = new Exam
            {
                Content = examDto.Content,
                Title = examDto.Title,
                UserId = examDto.UserId,
                CreatedAt = DateTime.Now,
                Medias = medias // Gán danh sách Media đã được xử lý
            };

            // Lưu exam vào database
            var createdExam = await _examService.AddAsync(exam);
            return CreatedAtAction(nameof(GetExamById), new { id = createdExam.Id }, createdExam);
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
                    UserId = examDto.UserId
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

        // [HttpGet("{mediaId}")]
        // public Task<ActionResult<ExamDto>> GetExamByMediaId(string mediaId)
        // {

        // }
    }
}
