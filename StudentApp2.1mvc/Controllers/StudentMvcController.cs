using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentLibrary.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace StudentApp2._1.Controllers
{
    public class StudentMvcController : Controller
    {
        private readonly HttpClient _client;

        public StudentMvcController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5298/api/"); // Update base URL as needed
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("Student");
            var json = await response.Content.ReadAsStringAsync();
            var students = JsonConvert.DeserializeObject<List<Student>>(json);
            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _client.GetAsync($"Student/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<Student>(json);
            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            var content = new MultipartFormDataContent();

            //content.Add(new StringContent(student.FirstName), "FirstName");
            //content.Add(new StringContent(student.LastName), "LastName");
            content.Add(new StringContent(student.RollNo), "RollNo");
            content.Add(new StringContent(student.FirstName), "FirstName");
            content.Add(new StringContent(student.LastName), "LastName");
            content.Add(new StringContent(student.FatherName), "FatherName");
            content.Add(new StringContent(student.DOB.ToString("yyyy-MM-dd")), "DOB");
            content.Add(new StringContent(student.MobileNo), "MobileNo");
            content.Add(new StringContent(student.EmailId), "EmailId");
            content.Add(new StringContent(student.Password), "Password");
            content.Add(new StringContent(student.Gender), "Gender");
            content.Add(new StringContent(student.Department), "Department");
            content.Add(new StringContent(student.Course), "Course");
            content.Add(new StringContent(student.City), "City");
            content.Add(new StringContent(student.Address), "Address");

            // File upload
            if (student.Photo != null && student.Photo.Length > 0)
            {
                var fileContent = new StreamContent(student.Photo.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(student.Photo.ContentType);
                content.Add(fileContent, "Photo", student.Photo.FileName);
            }

            // Add other fields and handle photo if needed

            var response = await _client.PostAsync("Student", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
                TempData["Error"]="Something went wrong";
                return View(student);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"Student/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var student = JsonConvert.DeserializeObject<Student>(json);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(student.RollNo), "RollNo");
            content.Add(new StringContent(student.FirstName), "FirstName");
            content.Add(new StringContent(student.LastName), "LastName");
            content.Add(new StringContent(student.FatherName), "FatherName");
            content.Add(new StringContent(student.DOB.ToString("yyyy-MM-dd")), "DOB");
            content.Add(new StringContent(student.MobileNo), "MobileNo");
            content.Add(new StringContent(student.EmailId), "EmailId");
            content.Add(new StringContent(student.Password), "Password");
            content.Add(new StringContent(student.Gender), "Gender");
            content.Add(new StringContent(student.Department), "Department");
            content.Add(new StringContent(student.Course), "Course");
            content.Add(new StringContent(student.City), "City");
            content.Add(new StringContent(student.Address), "Address");

            if (student.Photo != null && student.Photo.Length > 0)
            {
                var fileContent = new StreamContent(student.Photo.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(student.Photo.ContentType);
                content.Add(fileContent, "Photo", student.Photo.FileName);
            }

            var response = await _client.PutAsync($"Student/{id}", content);
            if (!response.IsSuccessStatusCode)
                return View(student);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"Student/{id}");
            return RedirectToAction("Index");
        }
    }
}