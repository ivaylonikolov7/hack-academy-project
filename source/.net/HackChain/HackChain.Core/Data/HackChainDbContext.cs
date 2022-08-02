using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Data
{
    public class HackChainDbContext : DbContext
    {
        public HackChainDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Block> Blocks { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Transaction>()
                .HasKey(t => t.Hash);

            builder.Entity<Account>()
                .HasKey(a => a.Address);

            builder.Entity<Block>()
                .HasKey(b => b.Id);
            builder.Entity<Block>()
                .HasIndex(b => b.Index)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
