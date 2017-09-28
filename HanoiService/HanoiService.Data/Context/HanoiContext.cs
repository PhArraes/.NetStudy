using HanoiService.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanoiService.Data.Context
{
    public class HanoiContext : DbContext
    {
        public HanoiContext()
            : base("name=DefaultConnection")
        { }

        public DbSet<HanoiLog> Logs { get; set; }

       
    }
}
