using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;

namespace BDRDExce.Infrastructures.Services
{
    public class SubmissionMediaService : BaseDbService<SubmissionMedia>, ISubmissionMediaService
    {
        public SubmissionMediaService(AppDbContext context) : base(context)
        {
        }

        public override Task<IEnumerable<SubmissionMedia>> GetAllAsync()
        {
            return base.GetAllAsync();
        }
    }
}