using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services
{
    public class ExamService : BaseDbService<Exam>, IExamService
    {
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
            existingExam.UserId = exam.UserId;

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
    }
}
