﻿using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

      
        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ApartmentServices> ApartmentServices { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserApartment> UserApartment { get; set; }

    }
}