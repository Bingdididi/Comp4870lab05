﻿using Code1stUsersRoles.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Code1stUsersRoles.Data;

public class ApplicationDbContext : IdentityDbContext<CustomUser, CustomRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);

    // Use seed method here
    // builder.Seed();
}
}
