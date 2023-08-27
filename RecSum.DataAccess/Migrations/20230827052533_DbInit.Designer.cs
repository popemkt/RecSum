﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecSum.DataAccess;

#nullable disable

namespace RecSum.DataAccess.Migrations
{
    [DbContext(typeof(RecSumContext))]
    [Migration("20230827052533_DbInit")]
    partial class DbInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("RecSum.Domain.Entities.Invoice", b =>
                {
                    b.Property<string>("Reference")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Cancelled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ClosedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorAddress1")
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorAddress2")
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorCountryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorReference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorRegistrationNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorState")
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorTown")
                        .HasColumnType("TEXT");

                    b.Property<string>("DebtorZip")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("OpeningValue")
                        .HasColumnType("REAL");

                    b.Property<double>("PaidValue")
                        .HasColumnType("REAL");

                    b.HasKey("Reference");

                    b.ToTable("Invoice", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
