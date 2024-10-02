using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BDRDExce.Infrastructures.Services.Interface
{
    public interface ICourseService : IBaseDbService<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByUserIdAsync(string userId);
        Task<CourseDto> AddCourse(CreateCourseDto createExamDto, HttpRequest request);
        Task<Course> UpdateCourseAsync(ChangeCourseDto courseDto, HttpRequest request);
    }
}