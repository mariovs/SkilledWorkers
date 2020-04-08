﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skills.Api.DataAccess;

namespace Skills.Api.DataAccess.Migrations
{
    [DbContext(typeof(SkillsContext))]
    partial class SkillsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Skills.Api.Models.Profession", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Name");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("Skills.Api.Models.ProfessionSkillsAvailable", b =>
                {
                    b.Property<string>("ProfessionName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("SkillLevelName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ProfessionName", "SkillLevelName");

                    b.ToTable("ProfessionSkillAvailable");
                });

            modelBuilder.Entity("Skills.Api.Models.SkillLevel", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Name");

                    b.ToTable("SkillLevels");
                });

            modelBuilder.Entity("Skills.Api.Models.Skills", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProfessionName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SkillLevelName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserSkillsUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionName");

                    b.HasIndex("SkillLevelName");

                    b.HasIndex("UserSkillsUserId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Skills.Api.Models.UserSkills", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("Skills.Api.Models.Skills", b =>
                {
                    b.HasOne("Skills.Api.Models.Profession", "Profession")
                        .WithMany()
                        .HasForeignKey("ProfessionName");

                    b.HasOne("Skills.Api.Models.SkillLevel", "SkillLevel")
                        .WithMany()
                        .HasForeignKey("SkillLevelName");

                    b.HasOne("Skills.Api.Models.UserSkills", null)
                        .WithMany("Skills")
                        .HasForeignKey("UserSkillsUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
