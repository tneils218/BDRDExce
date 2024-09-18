using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services
{
    public class ExamService : BaseDbService<Exam>, IExamService
    {
        private const long _maxFileSize = 5 * 1024 * 1024;
        public ExamService(AppDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Exam>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<Exam> GetByIdAsync(object id)
        {
            return await base.GetByIdAsync(id);
        }

        public override async Task<Exam> AddAsync(Exam exam)
        {
            exam.CreatedAt = DateTime.UtcNow;
            return await base.AddAsync(exam);
        }

        public override async Task<Exam> UpdateAsync(Exam exam)
        {
            var existingExam = await GetByIdAsync(exam.Id);
            if (existingExam == null)
            {
                throw new KeyNotFoundException("Exam not found.");
            }

            existingExam.Title = exam.Title;
            existingExam.Content = exam.Content;
            existingExam.Label = exam.Label;

            return await base.UpdateAsync(existingExam);
        }

        public override async Task DeleteAsync(object id)
        {
            var exam = await GetByIdAsync(id);
            if (exam == null)
            {
                throw new KeyNotFoundException("Exam not found.");
            }

            await base.DeleteAsync(exam);
        }

        public override async Task DeleteAsync(Exam exam)
        {
            await base.DeleteAsync(exam);
        }

        // Implement any additional methods declared in IExamService
        public async Task<IEnumerable<Exam>> GetExamsByUserIdAsync(string userId)
        {
            // Custom logic to retrieve exams by user ID
            return await _dbSet.Where(exam => exam.UserId == userId).ToListAsync();
        }

        public async Task<ExamDto> AddExam(CreateExamDto examDto, HttpRequest request)
        {
            var medias = await ProcessUploadedFiles(examDto.Files, request);
            var exam = new Exam
                {
                    Content = examDto.Content,
                    Title = examDto.Title,
                    UserId = examDto.UserId,
                    CreatedAt = DateTime.UtcNow,
                    Medias = medias,
                    Label = examDto.Label
                };
                await _dbSet.AddAsync(exam);
                await _context.SaveChangesAsync();
                return new ExamDto{Title = exam.Title, Content = exam.Content, Label = exam.Label};
        }

        private async Task<List<Media>> ProcessUploadedFiles(List<IFormFile> files, HttpRequest request)
        {
            var medias = new List<Media>();
            foreach (var file in files)
            {
                if (file.Length > 0 && file.Length <= _maxFileSize)
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
    }
}
