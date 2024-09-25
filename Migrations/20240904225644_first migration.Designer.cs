﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VotingApp.Context;

#nullable disable

namespace VotingApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240904225644_first migration")]
    partial class firstmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("VotingApp.Models.Entities.Candidate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.CandidatePosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CandidateId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("PositionId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Statement")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("PositionId");

                    b.ToTable("CandidatePositions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Election", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Elections");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ElectionId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("RuleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ElectionId");

                    b.HasIndex("RuleId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Rule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaxLevel")
                        .HasColumnType("int");

                    b.Property<decimal>("MinCGPA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MinLevel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Program")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("CGPA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("MatricNo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Program")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("92c9bc80-a640-40e2-8037-afc6a7ef0964"),
                            Email = "admin@gmail.com",
                            IsDeleted = false,
                            PasswordHash = "$2a$11$kg9kJS.3QIYnLWN64X3GXOGu7z9qG1rPJUKsut/48OcF71YYaoJGO",
                            Role = 1
                        });
                });

            modelBuilder.Entity("VotingApp.Models.Entities.VoteCastingInfo", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CandidatePositionId")
                        .HasColumnType("char(36)");

                    b.HasKey("StudentId", "CandidatePositionId");

                    b.HasIndex("CandidatePositionId");

                    b.ToTable("VoteCastingInfos");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Candidate", b =>
                {
                    b.HasOne("VotingApp.Models.Entities.Student", "Student")
                        .WithOne("Candidate")
                        .HasForeignKey("VotingApp.Models.Entities.Candidate", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.CandidatePosition", b =>
                {
                    b.HasOne("VotingApp.Models.Entities.Candidate", "Candidate")
                        .WithMany("CandidatePositions")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VotingApp.Models.Entities.Position", "Position")
                        .WithMany("CandidatePositions")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Election", b =>
                {
                    b.HasOne("VotingApp.Models.Entities.Session", "Session")
                        .WithMany("Elections")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Position", b =>
                {
                    b.HasOne("VotingApp.Models.Entities.Election", "Election")
                        .WithMany("Positions")
                        .HasForeignKey("ElectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VotingApp.Models.Entities.Rule", "Rule")
                        .WithMany("Positions")
                        .HasForeignKey("RuleId");

                    b.Navigation("Election");

                    b.Navigation("Rule");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.VoteCastingInfo", b =>
                {
                    b.HasOne("VotingApp.Models.Entities.CandidatePosition", "CandidatePosition")
                        .WithMany("Votes")
                        .HasForeignKey("CandidatePositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VotingApp.Models.Entities.Student", "Student")
                        .WithMany("Votes")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CandidatePosition");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Candidate", b =>
                {
                    b.Navigation("CandidatePositions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.CandidatePosition", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Election", b =>
                {
                    b.Navigation("Positions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Position", b =>
                {
                    b.Navigation("CandidatePositions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Rule", b =>
                {
                    b.Navigation("Positions");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Session", b =>
                {
                    b.Navigation("Elections");
                });

            modelBuilder.Entity("VotingApp.Models.Entities.Student", b =>
                {
                    b.Navigation("Candidate");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
