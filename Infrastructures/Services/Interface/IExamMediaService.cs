using BDRDExce.Models;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IExamMediaService : IBaseDbService<ExamMedia>
    {
        Task<ExamMedia> GetExamMediaById(int examId, string mediaId);
    }
}
