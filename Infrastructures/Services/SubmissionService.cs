using System.Linq.Expressions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;

namespace BDRDExce.Infrastructures.Services
{
    public class SubmissionService : BaseDbService<Submission>, ISubmissionService
    {
        public SubmissionService(AppDbContext context) : base(context)
        {
        }

        public override Task<Submission> AddAsync(Submission entity)
        {
            return base.AddAsync(entity);
        }

        public override Task DeleteAsync(object id)
        {
            return base.DeleteAsync(id);
        }

        public override Task<IEnumerable<Submission>> GetAllAsync()
        {
            return base.GetAllAsync();
            
        }

        public override Task<Submission> GetByIdAsync(object id)
        {
            return base.GetByIdAsync(id);
        }
    }
}