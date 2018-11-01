﻿// <auto-generated />
using System;
using DrawManager.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DrawManager.Api.Migrations
{
    [DbContext(typeof(DrawManagerDbContext))]
    partial class DrawManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DrawManager.Api.Entities.Draw", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AllowMultipleParticipations");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("ExecutedOn");

                    b.Property<string>("GroupName");

                    b.Property<string>("Name");

                    b.Property<DateTime>("ProgrammedFor");

                    b.HasKey("Id");

                    b.ToTable("Draws");
                });

            modelBuilder.Entity("DrawManager.Api.Entities.DrawEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DrawId");

                    b.Property<int>("EntrantId");

                    b.Property<DateTime>("RegisteredOn");

                    b.HasKey("Id");

                    b.HasIndex("DrawId");

                    b.HasIndex("EntrantId");

                    b.ToTable("DrawEntries");
                });

            modelBuilder.Entity("DrawManager.Api.Entities.Entrant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BranchOffice");

                    b.Property<string>("City");

                    b.Property<string>("Code");

                    b.Property<string>("Department");

                    b.Property<string>("Name");

                    b.Property<string>("Office");

                    b.Property<string>("Region");

                    b.Property<string>("SubDepartment");

                    b.Property<string>("Subsidiary");

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("Entrants");
                });

            modelBuilder.Entity("DrawManager.Api.Entities.Prize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttemptsUntilChooseWinner");

                    b.Property<string>("Description");

                    b.Property<int>("DrawId");

                    b.Property<DateTime?>("ExecutedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DrawId");

                    b.ToTable("Prizes");
                });

            modelBuilder.Entity("DrawManager.Api.Entities.PrizeSelectionStep", b =>
                {
                    b.Property<int>("PrizeId");

                    b.Property<int>("EntrantId");

                    b.Property<int>("PrizeSelectionStepType");

                    b.Property<DateTime>("RegisteredOn");

                    b.HasKey("PrizeId", "EntrantId");

                    b.HasIndex("EntrantId");

                    b.ToTable("PrizeSelectionSteps");
                });

            modelBuilder.Entity("DrawManager.Api.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Hash");

                    b.Property<string>("Login");

                    b.Property<byte[]>("Salt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DrawManager.Api.Entities.DrawEntry", b =>
                {
                    b.HasOne("DrawManager.Api.Entities.Draw", "Draw")
                        .WithMany("Entries")
                        .HasForeignKey("DrawId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DrawManager.Api.Entities.Entrant", "Entrant")
                        .WithMany("Entries")
                        .HasForeignKey("EntrantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DrawManager.Api.Entities.Prize", b =>
                {
                    b.HasOne("DrawManager.Api.Entities.Draw", "Draw")
                        .WithMany("Prizes")
                        .HasForeignKey("DrawId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DrawManager.Api.Entities.PrizeSelectionStep", b =>
                {
                    b.HasOne("DrawManager.Api.Entities.Entrant", "Entrant")
                        .WithMany("SelectionSteps")
                        .HasForeignKey("EntrantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DrawManager.Api.Entities.Prize", "Prize")
                        .WithMany("SelectionSteps")
                        .HasForeignKey("PrizeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
