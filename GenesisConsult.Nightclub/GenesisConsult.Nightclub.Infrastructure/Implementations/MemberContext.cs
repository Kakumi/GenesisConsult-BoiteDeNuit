using GenesisConsult.Nightclub.Application.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Infrastructure.Implementations
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions<MemberContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
    }
}
