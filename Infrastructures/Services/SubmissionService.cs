using BDRDExce.Commons;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services
{
    public class SubmissionService(AppDbContext context, IExamService examService) : BaseDbService<Submission>(context), ISubmissionService
    {

        public async Task<SubmissionDto> AddSubmission(CreateSubmissionDto submissionDto, HttpRequest request)
        {
            var submission = new Submission
            {
                ExamId = submissionDto.ExamId,
                UserId = submissionDto.UserId
            };
            var medias = new List<Media>();
            if(submissionDto.Content != null)
            {
                submission.Content = submissionDto.Content;
            }
            if(submissionDto.File != null)
            {
                var media = await Utils.ProcessUploadedFile(submissionDto.File, request);
                medias.Add(media);
            }
            submission.Medias = medias;
            await AddAsync(submission);
            var exam = await examService.GetByIdAsync(submissionDto.ExamId);
            exam.IsComplete = true;
            await examService.UpdateAsync(exam);

            return new SubmissionDto{Content = submission.Content, ExamId = submission.ExamId, UserId = submission.UserId};
        }

        public override async Task<IEnumerable<Submission>> GetAllAsync()
        {
            var result = await _dbSet.Include(x => x.Medias).ToListAsync();
            return result;
        }
        public override async Task<Submission> GetByIdAsync(object id)
        {
            var result =await _dbSet.Include(x => x.Medias).FirstOrDefaultAsync(x => x.Id == (int)id);
            return result;
        }
    }
}