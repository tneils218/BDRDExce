using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services
{
    public class CourseService : BaseDbService<Course>, ICourseService
    {
        
        public CourseService(AppDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<Course> GetByIdAsync(object id)
        {
            return await base.GetByIdAsync(id);
        }

        public override async Task<Course> AddAsync(Course course)
        {
            course.CreatedAt = DateTime.UtcNow;
            return await base.AddAsync(course);
        }

        public override async Task<Course> UpdateAsync(Course course)
        {
            var existingCourse = await GetByIdAsync(course.Id);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            existingCourse.Title = course.Title;
            existingCourse.Label = course.Label;

            return await base.UpdateAsync(existingCourse);
        }

        public override async Task DeleteAsync(object id)
        {
            var course = await GetByIdAsync(id);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            await base.DeleteAsync(course);
        }

        public override async Task DeleteAsync(Course course)
        {
            await base.DeleteAsync(course);
        }

        //Implement any additional methods declared in ICourseService
        public async Task<IEnumerable<Course>> GetCoursesByUserIdAsync(string userId)
        {
            // Custom logic to retrieve courses by user ID
            return await _dbSet.Where(course => course.UserId == userId).ToListAsync();
        }

        public async Task<CourseDto> AddCourse(CreateCourseDto courseDto, HttpRequest request)
        {
            var course = new Course
                {
                    Desc = courseDto.Desc,
                    Title = courseDto.Title,
                    UserId = courseDto.UserId,
                    CreatedAt = DateTime.UtcNow,
                    ImageUrl = courseDto.ImageUrl,
                    Label = courseDto.Label
                };
                await _dbSet.AddAsync(course);
                await _context.SaveChangesAsync();
                return new CourseDto{Title = course.Title, Label = course.Label};
        }

    }
}
