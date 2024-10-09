using BDRDExce.Models;
using BDRDExce.Models.DTOs;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface ISubmissionService : IBaseDbService<Submission>
    {
        Task<SubmissionDto> AddSubmission(CreateSubmissionDto submissionDto, HttpRequest request);
    }
}