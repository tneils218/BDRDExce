using System.Linq.Expressions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;

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
                using(var ms = new MemoryStream())
                {
                    await submissionDto.File.CopyToAsync(ms);
                    var fileByte = ms.ToArray();
                    var id = Guid.NewGuid().ToString();
                    var media = new Media
                    {
                        Id = id,
                        ContentType = submissionDto.File.ContentType,
                        ContentName = submissionDto.File.FileName,
                        Content = fileByte,
                        FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                    };
                    medias.Add(media);
                }
            }
            submission.Medias = medias;
            await AddAsync(submission);
            var exam = await examService.GetByIdAsync(submissionDto.ExamId);
            exam.IsComplete = true;
            await examService.UpdateAsync(exam);

            return new SubmissionDto{Content = submission.Content, ExamId = submission.ExamId, UserId = submission.UserId};
        }
    }
}