using Microsoft.EntityFrameworkCore;
using Opah_API.Infrastructure.Interfaces;
using Opah_API.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Opah_API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<TB_CIDADE> TB_CIDADE { get; set; }
        public DbSet<TB_CLIENTE> TB_CLIENTE { get; set; }
        public DbSet<TB_ENDERECO> TB_ENDERECO { get; set; }        
        public IDbConnection Connection => Database.GetDbConnection();
    }
}
