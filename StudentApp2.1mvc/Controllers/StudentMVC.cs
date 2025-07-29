using Microsoft.AspNetCore.Mvc;
using StudentLibrary.Models;
using StudentLibrary.Repositories;
using System.Threading.Tasks;

namespace StudentApp2._1mvc.Controllers
{
    public class StudentMVC : Controller
    {
        private readonly IStudentRepository _repo;
        private readonly HttpClient _client;

        public StudentMVC(IStudentRepository repo,IHttpClientFactory httpClientFactory)
        {
            _repo = repo;
            _client = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("http://localhost:5298/api/Student ");
            var data = await response.Content.ReadFromJsonAsync<List<Student>>();
            return View();
        }

        public async Task<IActionResult> Register(Student student)
        {
            
           

            if (student.Photo != null)
                student.PhotoPath = await _repo.SavePhotoAsync(student.Photo);
            student.FullName = student.FirstName + student.LastName;

            if (await _repo.MobileExistsAsync(student.MobileNo, student.StudentId))
            {
                ModelState.AddModelError("MobileNo", "Already Exists");
            }
            if (await _repo.EmailExistsAsync(student.EmailId, student.StudentId)) ModelState.AddModelError("EmailId", "Already Exists");

            if (ModelState.IsValid)
            {
                await _repo.AddAsync(student);
                return RedirectToAction("Index");
            }

            return View("Index", student);
        }

        public async Task<IActionResult> Read()
        {
            var students = await _repo.GetAllAsync();
            return View(students);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Read");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            try
            {
               

                if (student.Photo != null)
                    student.PhotoPath = await _repo.SavePhotoAsync(student.Photo);
                student.FullName = student.FirstName + student.LastName;

                if (await _repo.MobileExistsAsync(student.MobileNo, student.StudentId))
                {
                    ModelState.AddModelError("MobileNo", "Already Exists");
                }
                if (await _repo.EmailExistsAsync(student.EmailId, student.StudentId)) ModelState.AddModelError("EmailId", "Already Exists");


                if (!ModelState.IsValid)
                    return View(student);
                await _repo.UpdateAsync(student);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

        }
    }
}
