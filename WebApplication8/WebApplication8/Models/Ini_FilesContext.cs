using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication8.Models
{
    public class Ini_FilesContext : DbContext
    {
        public DbSet<Ini_Files> Ini_Files1 { get; set; }
        public Ini_FilesContext(DbContextOptions<Ini_FilesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
