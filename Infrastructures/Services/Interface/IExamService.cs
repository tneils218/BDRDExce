using BDRDExce.Models;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IExamService : IBaseDbService<Exam>
    {
        Task<IEnumerable<Exam>> GetExamsByUserIdAsync(string userId);
        void GetExamByMediaId(string mediaId);
    }
}