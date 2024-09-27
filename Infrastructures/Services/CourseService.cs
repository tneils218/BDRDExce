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
            var a =  await _dbSet.Include(x => x.Exams).ThenInclude(x => x.Medias).ToListAsync();
            return a;
        }

        public override async Task<Course> GetByIdAsync(object id)
        {
            var result = await _dbSet.Include(x => x.Exams).FirstOrDefaultAsync(x => x.Id == (int)id);
            return result;
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

        public async Task<IEnumerable<Course>> GetCoursesByUserIdAsync(string userId)
        {
            var a = await _dbSet.Where(course => course.UserId == userId).Include(x => x.Exams).ToListAsync();
            return a; 
        }

        public async Task<CourseDto> AddCourse(CreateCourseDto courseDto, HttpRequest request)
        {
            List<Media> medias = new List<Media>();
            using (var ms = new MemoryStream())
            {
                await courseDto.Image.CopyToAsync(ms);
                var fileBytes = ms.ToArray(); // Chuyển thành mảng byte
                var id = Guid.NewGuid().ToString();
                var media = new Media
                {
                    Id = id,
                    ContentType = courseDto.Image.ContentType,
                    ContentName = courseDto.Image.FileName,
                    Content = fileBytes,
                    FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                };
                medias.Add(media);
            }
            
            var course = new Course
            {
                Desc = courseDto.Desc,
                Title = courseDto.Title,
                UserId = courseDto.UserId,
                CreatedAt = DateTime.UtcNow,
                Medias = medias,
                ImageUrl = medias.FirstOrDefault().FileUrl,
                Label = courseDto.Label
            };
            await _dbSet.AddAsync(course);
            await _context.SaveChangesAsync();
            return new CourseDto{Title = course.Title, Label = course.Label};
        }
    }
}
