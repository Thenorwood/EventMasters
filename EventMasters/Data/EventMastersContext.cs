using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventMasters.Models;

namespace EventMasters.Data
{
    public class EventMastersContext : DbContext
    {
        public EventMastersContext (DbContextOptions<EventMastersContext> options)
            : base(options)
        {
        }

        public DbSet<EventMasters.Models.Concert> Concert { get; set; } = default!;
        public DbSet<EventMasters.Models.Category> Category { get; set; } = default!;

        public DbSet<Purchase> Purchase { get; set; } = default!;
    }
}
