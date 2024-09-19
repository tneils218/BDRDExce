using BDRDExce.Models;
using BDRDExce.Models.DTOs;

namespace BDRDExce.Infrastructures.Services.Interface;

public interface IExamService : IBaseDbService<Exam>
{
    Task<IEnumerable<ExamDto>> GetAllExam();
    Task<ExamDto> AddExam(CreateExamDto createExamDto, HttpRequest request);
    Task<IEnumerable<ExamDto>> GetExamsByCourseId(int courseId);
}