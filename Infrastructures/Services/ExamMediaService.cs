using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services
{
    public class ExamMediaService : BaseDbService<ExamMedia>, IExamMediaService
    {
        public ExamMediaService(AppDbContext context) : base(context)
        {

        }
        public override async Task<ExamMedia> AddAsync(ExamMedia entity)
        {
            return await base.AddAsync(entity);
        }

        public override async Task<IEnumerable<ExamMedia>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public override Task<ExamMedia> UpdateAsync(ExamMedia entity)
        {
            return base.UpdateAsync(entity);
        }

        public override Task DeleteAsync(object id)
        {
            return base.DeleteAsync(id);
        }

        public async Task<ExamMedia> GetExamMediaById(int examId, string mediaId)
        {
            var examMedia = await _dbSet.FindAsync(examId, mediaId);
            return examMedia;
        }

    }
}