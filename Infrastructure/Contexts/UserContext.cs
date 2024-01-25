using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Infrastructure.Contexts;

public partial class UserContext : DbContext
{
    /// <summary>
    /// Empty constructors and then the properties
    /// </summary>
    public UserContext()
    {

    }

    public UserContext(DbContextOptions<UserContext> options) : base(options)
    { 

    }

        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<AddressEntity> Addresses { get; set; }
        public virtual DbSet<ContactInformationEntity> ContactInformations { get; set; }
        public virtual DbSet<RoleEntity> Roles { get; set; }
        public virtual DbSet<AuthenticationEntity> Authenthications { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<UserRoleEntity>().HasKey(x => new { x.UserId, x.RoleId });
        //}
}

       

