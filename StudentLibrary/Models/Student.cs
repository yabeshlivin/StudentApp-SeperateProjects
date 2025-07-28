using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentLibrary.Models
{
   public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public string RollNo { get; set; }
        [Required]
        [NotMapped]
        public string FirstName { get; set; }
        [Required]
        [NotMapped]
        public string LastName { get; set; }
        public string FullName { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$")]

        public string MobileNo { get; set; }
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string Course { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

        public string PhotoPath { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
