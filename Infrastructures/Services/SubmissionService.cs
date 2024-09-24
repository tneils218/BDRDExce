using System.Linq.Expressions;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;

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
            return new SubmissionDto{Content = submission.Content, ExamId = submission.ExamId, UserId = submission.UserId};
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