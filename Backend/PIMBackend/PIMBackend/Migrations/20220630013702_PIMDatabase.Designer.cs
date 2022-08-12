﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PIMBackend.Database;

namespace PIMBackend.Migrations
{
    [DbContext(typeof(PIMContext))]
    [Migration("20220630013702_PIMDatabase")]
    partial class PIMDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EmployeeProject", b =>
                {
                    b.Property<decimal>("EmployeesId")
                        .HasColumnType("numeric(19,0)");

                    b.Property<decimal>("ProjectsId")
                        .HasColumnType("numeric(19,0)");

                    b.HasKey("EmployeesId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("EmployeeProject");
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Employee", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(19,0)")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("BIRTH_DATE");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("FIRST_NAME");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("LAST_NAME");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("VERSION");

                    b.Property<string>("Visa")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("char(3)")
                        .HasColumnName("VISA")
                        .IsFixedLength(true);

                    b.HasKey("Id");

                    b.ToTable("EMPLOYEE");
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Group", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(19,0)")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("GroupLeaderId")
                        .HasColumnType("numeric(19,0)")
                        .HasColumnName("GROUP_LEADER_ID");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("VERSION");

                    b.HasKey("Id");

                    b.HasIndex("GroupLeaderId")
                        .IsUnique();

                    b.ToTable("GROUP");
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Project", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(19,0)")
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Customer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("CUSTOMER");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("END_DATE");

                    b.Property<decimal>("GroupId")
                        .HasColumnType("numeric(19,0)")
                        .HasColumnName("GROUP_ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("NAME");

                    b.Property<decimal>("ProjectNumber")
                        .HasColumnType("numeric(4,0)")
                        .HasColumnName("PROJECT_NUMBER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("START_DATE");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("char(3)")
                        .HasColumnName("STATUS")
                        .IsFixedLength(true);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("VERSION");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ProjectNumber")
                        .IsUnique();

                    b.ToTable("PROJECT");
                });

            modelBuilder.Entity("EmployeeProject", b =>
                {
                    b.HasOne("PIMBackend.Domain.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PIMBackend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Group", b =>
                {
                    b.HasOne("PIMBackend.Domain.Entities.Employee", "GroupLeader")
                        .WithOne("Group")
                        .HasForeignKey("PIMBackend.Domain.Entities.Group", "GroupLeaderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GroupLeader");
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Project", b =>
                {
                    b.HasOne("PIMBackend.Domain.Entities.Group", "Group")
                        .WithMany("Projects")
                        .HasForeignKey("GroupId")
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Employee", b =>
                {
                    b.Navigation("Group");
                });

            modelBuilder.Entity("PIMBackend.Domain.Entities.Group", b =>
                {
                    b.Navigation("Projects");
                });
#pragma warning restore 612, 618
        }
    }
}
