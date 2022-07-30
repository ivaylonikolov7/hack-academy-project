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

            builder.Entity<Transaction>()
                .Property(t => t.Value)
                .HasColumnType<decimal>("decimal(18,0)");


            builder.Entity<Transaction>()
                .Property(t => t.Fee)
                .HasColumnType<decimal>("decimal(18,0)");


            builder.Entity<Account>()
                .HasKey(a => a.Address);

            builder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType<decimal>("decimal(18,0)");


            builder.Entity<Block>()
                .HasKey(b => b.Index);


            base.OnModelCreating(builder);
        }
    }
}
