﻿using System;
using System.Collections.Generic;
using DBFirstDemo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Models;

public partial class SoftUniContext : DbContext
{
    public SoftUniContext()
    {
    }

    public SoftUniContext(DbContextOptions<SoftUniContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<Department> Departments { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;
    public virtual DbSet<Project> Projects { get; set; } = null!;
    public virtual DbSet<RichEmployee> RichEmployees { get; set; } = null!;
    public virtual DbSet<Town> Towns { get; set; } = null!;
    public virtual DbSet<VEmployeeNameJobTitle> VEmployeeNameJobTitles { get; set; } = null!;
    public virtual DbSet<VEmployeesHiredAfter2000> VEmployeesHiredAfter2000s { get; set; } = null!;
    public virtual DbSet<VEmployeesSalary> VEmployeesSalaries { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=DESKTOP-PI68CL1\\MSSQLSERVER01;Database=SoftUni;Integrated Security=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.Property(e => e.AddressId).HasColumnName("AddressID");

            entity.Property(e => e.AddressText)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.TownId).HasColumnName("TownID");

            entity.HasOne(d => d.Town)
                .WithMany(p => p.Addresses)
                .HasForeignKey(d => d.TownId)
                .HasConstraintName("FK_Addresses_Towns");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Manager)
                .WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Employees");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.HireDate).HasColumnType("smalldatetime");

            entity.Property(e => e.JobTitle)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Salary).HasColumnType("money");

            entity.HasOne(d => d.Address)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Employees_Addresses");

            entity.HasOne(d => d.Department)
                .WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Departments");

            entity.HasOne(d => d.Manager)
                .WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Employees_Employees");

            entity.HasMany(d => d.Projects)
                .WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeesProject",
                    l => l.HasOne<Project>().WithMany().HasForeignKey("ProjectId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeesProjects_Projects"),
                    r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_EmployeesProjects_Employees"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId");

                        j.ToTable("EmployeesProjects");

                        j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                        j.IndexerProperty<int>("ProjectId").HasColumnName("ProjectID");
                    });
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.Property(e => e.Description).HasColumnType("ntext");

            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<RichEmployee>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AddressId).HasColumnName("AddressID");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("EmployeeID");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.HireDate).HasColumnType("smalldatetime");

            entity.Property(e => e.JobTitle)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Salary).HasColumnType("money");
        });

        modelBuilder.Entity<Town>(entity =>
        {
            entity.Property(e => e.TownId).HasColumnName("TownID");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VEmployeeNameJobTitle>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("V_EmployeeNameJobTitle");

            entity.Property(e => e.FullName)
                .HasMaxLength(152)
                .IsUnicode(false)
                .HasColumnName("Full Name");

            entity.Property(e => e.JobTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Job Title");
        });

        modelBuilder.Entity<VEmployeesHiredAfter2000>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("V_EmployeesHiredAfter2000");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VEmployeesSalary>(entity =>
        {
            entity.HasNoKey();

            entity.ToView("V_EmployeesSalaries");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Salary).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}