using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models
{
    public class RangoMediaContextContextFactory : IDesignTimeDbContextFactory<RangoMediaContext>
    {
        public RangoMediaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RangoMediaContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=RangoMedia;Trusted_Connection=true;MultipleActiveResultSets=true;Encrypt=False;");

            return new RangoMediaContext(optionsBuilder.Options);
        }
    }
}
