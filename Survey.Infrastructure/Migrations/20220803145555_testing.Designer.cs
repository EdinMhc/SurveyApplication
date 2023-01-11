﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Survey.Infrastructure.ContextClass1;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    [DbContext(typeof(ContextClass))]
    [Migration("20220803145555_testing")]
    partial class testing
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "cf04c817-c802-41ee-8c84-0c24c1596735",
                            ConcurrencyStamp = "a45121ea-0118-4583-8696-79d563073118",
                            Name = "SuperAdmin",
                            NormalizedName = "SuperAdmin"
                        },
                        new
                        {
                            Id = "7a921cf5-ffc8-445b-aaa1-964f0d15dfbc",
                            ConcurrencyStamp = "57006a20-9d08-42a4-bb1b-3f930ef0af7a",
                            Name = "Admin",
                            NormalizedName = "Admin"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Anwser", b =>
                {
                    b.Property<int>("AnwserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AnwserID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnwserID"), 1L, 1);

                    b.Property<int>("AnwserBlockID")
                        .HasColumnType("int");

                    b.Property<string>("AnwserText")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("AnwserID");

                    b.HasIndex("AnwserBlockID");

                    b.ToTable("Anwser");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.AnwserBlock", b =>
                {
                    b.Property<int>("AnwserBlockID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AnwserBlockID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnwserBlockID"), 1L, 1);

                    b.Property<string>("AnwserBlockName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BlockType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("CodeOfAnwserBlock")
                        .HasColumnType("int");

                    b.Property<int?>("CompanyID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("AnwserBlockID");

                    b.HasIndex("CompanyID");

                    b.ToTable("AnwserBlock");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Company", b =>
                {
                    b.Property<int>("CompanyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CompanyID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyID"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CompanyID");

                    b.HasIndex("UserID");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.JwtRelated.RefreshToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Invalidated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Token");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Question", b =>
                {
                    b.Property<int>("QuestionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("QuestionID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionID"), 1L, 1);

                    b.Property<int>("AnwserBlockID")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("SurveyID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("QuestionID");

                    b.HasIndex("AnwserBlockID");

                    b.HasIndex("SurveyID");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.SurveyReport", b =>
                {
                    b.Property<int>("SurveyReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SurveyReportID"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("SurveyID")
                        .HasColumnType("int");

                    b.HasKey("SurveyReportID");

                    b.HasIndex("SurveyID");

                    b.ToTable("SurveyReport");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.SurveyReportData", b =>
                {
                    b.Property<int>("RespondentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RespondentId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RespondentID"), 1L, 1);

                    b.Property<int>("AnswerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("QuestionID")
                        .HasMaxLength(100)
                        .HasColumnType("int");

                    b.Property<int>("SurveyReportID")
                        .HasColumnType("int");

                    b.HasKey("RespondentID");

                    b.HasIndex("SurveyReportID");

                    b.ToTable("surveyReportData");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Surveys", b =>
                {
                    b.Property<int>("SurveyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SurveyID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SurveyID"), 1L, 1);

                    b.Property<int?>("CompanyID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsActive")
                        .HasMaxLength(255)
                        .HasColumnType("bit");

                    b.Property<string>("SurveyName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("SurveyID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Survey");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "39ea42fb-2230-4998-b83c-5d5a2e4f70d2",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "3b629003-4a2d-4667-b28b-f3228cccb321",
                            Email = "edin.muhic@forsta.com",
                            EmailConfirmed = false,
                            FirstName = "Edin",
                            LastName = "Muhic",
                            LockoutEnabled = false,
                            NormalizedEmail = "EDIN.MUHIC@FORSTA.COM",
                            NormalizedUserName = "EDIN.MUHIC@FORSTA.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "2d27aecb-2aa6-4649-9073-1740a57ede37",
                            TwoFactorEnabled = false,
                            UserName = "edin.muhic@forsta.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Survey.Infrastructure.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Anwser", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.AnwserBlock", "AnwserBlock")
                        .WithMany("Anwsers")
                        .HasForeignKey("AnwserBlockID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnwserBlock");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.AnwserBlock", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.Company", "Company")
                        .WithMany("AnswerBlock")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Company", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.User", "User")
                        .WithMany("Company")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.JwtRelated.RefreshToken", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Question", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.AnwserBlock", "AnwserBlock")
                        .WithMany("Questions")
                        .HasForeignKey("AnwserBlockID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Survey.Infrastructure.Entities.Surveys", "Survey")
                        .WithMany("Questions")
                        .HasForeignKey("SurveyID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AnwserBlock");

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.SurveyReport", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.Surveys", "Survey")
                        .WithMany("SurveyReport")
                        .HasForeignKey("SurveyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.SurveyReportData", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.SurveyReport", "SurveyReport")
                        .WithMany("SurveyReportData")
                        .HasForeignKey("SurveyReportID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SurveyReport");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Surveys", b =>
                {
                    b.HasOne("Survey.Infrastructure.Entities.Company", "Company")
                        .WithMany("Surveys")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.AnwserBlock", b =>
                {
                    b.Navigation("Anwsers");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Company", b =>
                {
                    b.Navigation("AnswerBlock");

                    b.Navigation("Surveys");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.SurveyReport", b =>
                {
                    b.Navigation("SurveyReportData");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.Surveys", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("SurveyReport");
                });

            modelBuilder.Entity("Survey.Infrastructure.Entities.User", b =>
                {
                    b.Navigation("Company");
                });
#pragma warning restore 612, 618
        }
    }
}
