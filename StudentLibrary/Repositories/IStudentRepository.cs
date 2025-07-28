using Microsoft.AspNetCore.Http;
using StudentLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary.Repositories
{
   public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
        Task<string> SavePhotoAsync(IFormFile photo);
        Task<bool> EmailExistsAsync(string email, int studentId);
        Task<bool> MobileExistsAsync(string mobile, int studentId);
      
    }
}
