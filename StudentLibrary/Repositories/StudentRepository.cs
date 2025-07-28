using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentLibrary.Data;
using StudentLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary.Repositories
{
    public class StudentRepository:IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public StudentRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Student>> GetAllAsync() => await _context.Students.ToListAsync();

        public async Task<Student> GetByIdAsync(int id) => await _context.Students.FindAsync(id);

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> SavePhotoAsync(IFormFile photo)
        {
            if (photo == null) return null;

            var folder = Path.Combine(_env.WebRootPath, "images");
            var filename = Guid.NewGuid().ToString() + "_" + photo.FileName;
            var filepath = Path.Combine(folder, filename);

            using var stream = new FileStream(filepath, FileMode.Create);
            await photo.CopyToAsync(stream);

            return "/images/" + filename;
        }

        public async Task<bool> EmailExistsAsync(string email, int studentId)
        {
            return await _context.Students.AnyAsync(s => s.EmailId == email && s.StudentId != studentId);
        }
        public async Task<bool> MobileExistsAsync(string mobile, int studentId)
        {
            return await _context.Students.AnyAsync(s => s.MobileNo == mobile && s.StudentId != studentId);
        }
    }
}
