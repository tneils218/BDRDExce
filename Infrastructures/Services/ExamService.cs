using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services;

public class ExamService(AppDbContext context) : BaseDbService<Exam>(context), IExamService
{
    private readonly string[] _allowedContentTypes = {".jpg", ".png", ".jpeg", ".rar", ".zip"};
    private const long _maxFileSize = 10 * 1024 * 1024;

    public async Task<ExamDto> AddExam(CreateExamDto createExamDto, HttpRequest request)
    {
        var medias =new List<Media>();
        if(createExamDto.Files != null)
        {
            medias = await ProcessUploadedFiles(createExamDto.Files, request);
        }
        var exam = new Exam{
            CourseId = createExamDto.CourseId,
            Content = createExamDto.Content,
            Title = createExamDto.Title,
            Medias = medias,
            IsComplete = false
        };
        await _dbSet.AddAsync(exam);
        await _context.SaveChangesAsync();
        return new ExamDto(exam.Id, exam.Title, exam.Content, exam.CourseId, exam.IsComplete);
    }

    public async Task<IEnumerable<ExamDto>> GetAllExam()
    {
        var exams = await _dbSet.ToListAsync();
        var examDto = exams.Select(e => {
            return new ExamDto(e.Id, e.Title, e.Content, e.CourseId, e.IsComplete);
        });
        return examDto;
    }

    public override Task DeleteAsync(object id)
    {
        return base.DeleteAsync(id);
    }

    public async Task<IEnumerable<ExamDto>> GetExamsByCourseId(int courseId)
    {
        var exams = await _dbSet.Where(e => e.CourseId == courseId).ToListAsync();
        var examDtos = exams.Select(x => {
            return new ExamDto(x.Id, x.Title, x.Content, x.CourseId, x.IsComplete);
        });
        return examDtos;
    }
    private async Task<List<Media>> ProcessUploadedFiles(List<IFormFile> files, HttpRequest request)
    {
        var medias = new List<Media>();
        foreach (var file in files)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (file.Length > 0 && file.Length <= _maxFileSize && _allowedContentTypes.Contains(fileExtension))
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    var id = Guid.NewGuid().ToString();
                    var media = new Media
                    {
                        Id = id,
                        ContentType = file.ContentType,
                        ContentName = file.FileName,
                        Content = fileBytes,
                        FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                    };
                    medias.Add(media);
                }
            }
        }
        return medias;
    }

    public override async Task<Exam> UpdateAsync(Exam entity)
    {
        var exam = await _dbSet.FindAsync(entity.Id);
        if(exam != null)
        {
            exam.Content = entity.Content;
            exam.Title = entity.Title;
            exam.CourseId = entity.CourseId;
            exam.IsComplete = entity.IsComplete;
            return await base.UpdateAsync(exam);
        }
        return null;
    }
}
