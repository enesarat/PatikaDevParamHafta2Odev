using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatikaDevParamHafta2Odev.Entity.Concrete.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.Context
{
    public class UsersContext : IdentityUserContext<IdentityUser>
    {
        public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
        {
        }
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RegistrationRequest>().HasData(
                new RegistrationRequest()
                {
                    Id = 1,
                    Username= "test",
                    Password= "testtest",
                    Email = "test@gmail.com"

                },
                new RegistrationRequest()
                {
                    Id = 2,
                    Email = "enes_arat@outlook.com",
                    Password = "password",
                    Username = "enesarat"
                }
                );
           
        }
    }
}
