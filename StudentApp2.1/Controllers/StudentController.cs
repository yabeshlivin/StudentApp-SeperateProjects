using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentLibrary.Models;
using StudentLibrary.Repositories;

namespace StudentApp2._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repo;

        public StudentController(IStudentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _repo.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Student student)
        {
           

            if (student.Photo != null)
                student.PhotoPath = await _repo.SavePhotoAsync(student.Photo);

            student.FullName = student.FirstName + student.LastName;

            if (await _repo.MobileExistsAsync(student.MobileNo, student.StudentId))
            {
                ModelState.AddModelError("MobileNo", "Already Exists");
            }
            if (await _repo.EmailExistsAsync(student.EmailId, student.StudentId))
            {
                ModelState.AddModelError("EmailId", "Already Exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.AddAsync(student);
            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Student student)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.RollNo = student.RollNo;
            existing.FirstName = student.FirstName;
            existing.LastName = student.LastName;
            existing.FatherName = student.FatherName;
            existing.DOB = student.DOB;
            existing.MobileNo = student.MobileNo;
            existing.EmailId = student.EmailId;
            existing.Password = student.Password;
            existing.Gender = student.Gender;
            existing.Department = student.Department;
            existing.Course = student.Course;
            existing.City = student.City;
            existing.Address = student.Address;

            if (student.Photo != null)
                existing.PhotoPath = await _repo.SavePhotoAsync(student.Photo);

            existing.FullName = student.FirstName + student.LastName;

            if (await _repo.MobileExistsAsync(student.MobileNo, student.StudentId))
            {
                ModelState.AddModelError("MobileNo", "Already Exists");
            }
            if (await _repo.EmailExistsAsync(student.EmailId, student.StudentId))
            {
                ModelState.AddModelError("EmailId", "Already Exists");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _repo.DeleteAsync(id);
            return Ok(new { message = "Student deleted successfully" });
        }
    }
}