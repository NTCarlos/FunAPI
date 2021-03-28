using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Setting> Settings { get; set; }
    }
}
