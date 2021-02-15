using foodtruacker.Authentication.Configuration;
using foodtruacker.Authentication.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace foodtruacker.Authentication.Repository
{
    public class IdentityContext : IdentityDbContext<User, Role, Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(new List<Role>
            {
                new Role {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = FixedRoles.AdminRole
                },
                new Role {
                    Id = Guid.NewGuid(),
                    Name = "Customer",
                    NormalizedName = FixedRoles.CustomerRole
                },
            });
        }
    }
}
