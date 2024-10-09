using BDRDExce.Commons;
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
            var a =  await _dbSet.Include(x => x.Exams).ToListAsync();
            return a;
        }

        public override async Task<Course> GetByIdAsync(object id)
        {
            var result = await _dbSet
            .Include(x => x.Exams).ThenInclude(x => x.Medias)
            .Include(x => x.Exams).ThenInclude(x => x.Submissions).ThenInclude(x => x.Medias)
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == (int)id);
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

        public async Task<IEnumerable<CourseDto>> GetCoursesByUserIdAsync(string userId)
        {
            var courses = await _dbSet.Where(course => course.UserId == userId)
            .Include(x => x.Exams)
            .ThenInclude(x => x.Medias)
            .Include(x => x.Exams)
            .ThenInclude(x => x.Submissions).ThenInclude(x => x.Medias)
            .Include(x => x.Media)
            .ToListAsync();
            var courseDto = courses.Select(c => 
            {
                var file = c.Media!= null ? new FileDto(c.Media.ContentName, c.Media.FileUrl) : new FileDto();

                return new CourseDto { 
                    Id = c.Id, 
                    Title =
                    c.Title,
                    Desc = c.Desc,
                    Label = c.Label, 
                    File = file, 
                    Exams = c.Exams.Select(e =>
                    {
                        var filesSubmission = e.Submissions.Where(x => x.UserId == userId)
                        .SelectMany(u => u.Medias).Select(x => {return new FileDto(x.ContentName, x.FileUrl);})
                        .ToList();
                        var filesExam = e.Medias.Select(x => {return new FileDto(x.ContentName, x.FileUrl);}).ToList();

                        return new ExamDto(e.Id, e.Title, e.Content, e.CourseId, e.IsComplete, filesExam, filesSubmission);
                    }).ToList() };
            });
            return courseDto; 
        }

        public async Task<CourseDto> AddCourse(CreateCourseDto courseDto, HttpRequest request)
        {
            Media media = null;
            if(courseDto.Image != null)
            {
                media = await Utils.ProcessUploadedFile(courseDto.Image, request);
            }
            var course = new Course
            {
                Desc = courseDto.Desc,
                Title = courseDto.Title,
                UserId = courseDto.UserId,
                CreatedAt = DateTime.UtcNow,
                Media = media,
                ImageUrl = media != null ? media.FileUrl : null,
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
                media = await Utils.ProcessUploadedFile(courseDto.Image, request);
            }
            course.Desc = courseDto.Desc ?? course.Desc;
            course.Title = courseDto.Title ?? course.Title;
            course.Label = courseDto.Label ?? course.Label;
            course.Media = media ?? course.Media;
            course.ImageUrl = media != null ? media.FileUrl : course.ImageUrl;
            _dbSet.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }
    }
}
