using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface IExamService : IBaseDbService<Exam>
    {
        Task<IEnumerable<Exam>> GetExamsByUserIdAsync(string userId);
        Task<ExamDto> AddExam(CreateExamDto createExamDto, HttpRequest request);
    }
}