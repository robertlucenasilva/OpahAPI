using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Opah_API.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Opah_API.Infrastructure.Interfaces
{
    public interface IApplicationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        public DbSet<TB_CIDADE> TB_CIDADE { get; set; }
        public DbSet<TB_CLIENTE> TB_CLIENTE { get; set; }
        public DbSet<TB_ENDERECO> TB_ENDERECO { get; set; }        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
