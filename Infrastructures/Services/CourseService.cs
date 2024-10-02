using System.Reflection.Metadata.Ecma335;
using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;
using BDRDExce.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BDRDExce.Infrastructures.Services
{
    public class CourseService(AppDbContext context) : BaseDbService<Course>(context), ICourseService
    {
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
            existingCourse.Desc = course.Desc;
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
            Media media = null;
            using (var ms = new MemoryStream())
            {
                await courseDto.Image.CopyToAsync(ms);
                var fileBytes = ms.ToArray(); // Chuyển thành mảng byte
                var id = Guid.NewGuid().ToString();
                media = new Media
                {
                    Id = id,
                    ContentType = courseDto.Image.ContentType,
                    ContentName = courseDto.Image.FileName,
                    Content = fileBytes,
                    FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                };
            }
            
            var course = new Course
            {
                Desc = courseDto.Desc,
                Title = courseDto.Title,
                UserId = courseDto.UserId,
                CreatedAt = DateTime.UtcNow,
                Media = media,
                ImageUrl = media.FileUrl,
                Label = courseDto.Label
            };
            await _dbSet.AddAsync(course);
            await _context.SaveChangesAsync();
            return new CourseDto{Title = course.Title, Label = course.Label};
        }

        public async Task<Course> UpdateCourseAsync(ChangeCourseDto courseDto, HttpRequest request)
        {
            var course =await _dbSet.FindAsync(courseDto.Id);
            if(course == null)
            {
                return null;
            }
            Media media = null;
            if(courseDto.Image != null)
            {
                using(var ms = new MemoryStream())
                {
                    await courseDto.Image.CopyToAsync(ms);
                    var fileBytes = ms.ToArray(); // Chuyển thành mảng byte
                    var id = Guid.NewGuid().ToString();
                    media = new Media
                    {
                        Id = id,
                        ContentType = courseDto.Image.ContentType,
                        ContentName = courseDto.Image.FileName,
                        Content = fileBytes,
                        FileUrl = $"{request.Scheme}://{request.Host}/api/v1/Media/{id}"
                    };
                }
            }
            course.Desc = courseDto.Desc == null ? course.Desc : courseDto.Desc;
            course.Title = courseDto.Title == null ? course.Title : courseDto.Title;
            course.Label = courseDto.Label == null ? course.Label : courseDto.Label;
            course.Media = media;
            course.ImageUrl = media != null ? media.FileUrl : course.ImageUrl;
            _dbSet.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }
    }
}
