using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ECore.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ECore.Domain.Context
{
    public class EcoreDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<CardsCollection> CardsCollection { get; set; }
        public virtual DbSet<Item> Item { get; set; }

        public EcoreDbContext(DbContextOptions<EcoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
