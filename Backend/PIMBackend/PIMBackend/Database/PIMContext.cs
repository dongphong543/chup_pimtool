﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PIMBackend.Domain.Entities;

using System.Data.Common;

#nullable disable

namespace PIMBackend.Database
{
    public partial class PIMContext: DbContext
    {
        public PIMContext()
        {
        }

        public PIMContext(DbContextOptions<PIMContext> options)
            : base(options)
        {
        }

        //public PIMContext(DbConnection dbConnection)
        //    : base(dbConnection)
        //{
        //}

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=PC22787;Initial Catalog=PIM_database;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("EMPLOYEE");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(19, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("BIRTH_DATE");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LAST_NAME");

                entity.Property(e => e.Version)
                    .HasColumnName("VERSION")
                    .IsRowVersion();

                entity.Property(e => e.Visa)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VISA")
                    .IsFixedLength(true);

                modelBuilder.Entity<Employee>()
                    .HasOne<Group>(s => s.Group)
                    .WithOne(g => g.GroupLeader)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasForeignKey<Group>(s => s.GroupLeaderId);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("GROUP");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(19, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.GroupLeaderId)
                    .HasColumnType("numeric(19, 0)")
                    .HasColumnName("GROUP_LEADER_ID");

                entity.Property(e => e.Version)
                    .HasColumnName("VERSION")
                    .IsRowVersion();

            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("PROJECT");

                entity.HasIndex(e => e.ProjectNumber)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(19, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.GroupId)
                    .HasColumnType("numeric(19, 0)")
                    .HasColumnName("GROUP_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.ProjectNumber)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("PROJECT_NUMBER");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.Version)
                    .HasColumnName("VERSION")
                    .IsRowVersion();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
