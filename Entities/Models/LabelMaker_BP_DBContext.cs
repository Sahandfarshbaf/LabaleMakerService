using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entities.Models
{
    public partial class LabelMaker_BP_DBContext : DbContext
    {
        public LabelMaker_BP_DBContext()
        {
        }

        public LabelMaker_BP_DBContext(DbContextOptions<LabelMaker_BP_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Commodity> Commodities { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserActivationCode> UserActivationCodes { get; set; } = null!;
        public virtual DbSet<UserLogin> UserLogins { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commodity>(entity =>
            {
                entity.ToTable("Commodity");

                entity.HasIndex(e => e.CommodityId, "NonClusteredIndex-CommodityId_Unique")
                    .IsUnique();

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserId");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserId");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserId");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.HasOne(d => d.Cuser)
                    .WithMany(p => p.CommodityCusers)
                    .HasForeignKey(d => d.CuserId)
                    .HasConstraintName("FK_Commodity_Users");

                entity.HasOne(d => d.DaUser)
                    .WithMany(p => p.CommodityDaUsers)
                    .HasForeignKey(d => d.DaUserId)
                    .HasConstraintName("FK_Commodity_Users3");

                entity.HasOne(d => d.Duser)
                    .WithMany(p => p.CommodityDusers)
                    .HasForeignKey(d => d.DuserId)
                    .HasConstraintName("FK_Commodity_Users2");

                entity.HasOne(d => d.Muser)
                    .WithMany(p => p.CommodityMusers)
                    .HasForeignKey(d => d.MuserId)
                    .HasConstraintName("FK_Commodity_Users1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.Email).HasMaxLength(128);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Hpassword)
                    .HasMaxLength(512)
                    .HasColumnName("HPassword");

                entity.Property(e => e.NationalCode)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<UserActivationCode>(entity =>
            {
                entity.ToTable("UserActivationCode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserActivationCodes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserActivationCode_Users");
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("UserLogin");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.LoginBrowser).HasMaxLength(128);

                entity.Property(e => e.LoginIp)
                    .HasMaxLength(512)
                    .HasColumnName("LoginIP");

                entity.Property(e => e.LoginOs).HasMaxLength(128);

                entity.Property(e => e.LoginPassword).HasMaxLength(512);

                entity.Property(e => e.LoginType).HasComment("1 = Success\r\n2 = Failed");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserLogin_Users");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRole_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
