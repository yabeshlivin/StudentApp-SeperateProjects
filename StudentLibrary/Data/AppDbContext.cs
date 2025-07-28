using Microsoft.EntityFrameworkCore;
using StudentLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Constraints
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.EmailId)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.MobileNo)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }   }
}
