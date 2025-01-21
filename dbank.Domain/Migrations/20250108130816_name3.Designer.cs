﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using DBank.Domain;

#nullable disable

namespace DBank.Domain.Migrations
{
    [DbContext(typeof(BankDbContext))]
    [Migration("20250108130816_name3")]
    partial class name3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("dbank.Web.Domain.Entities.BalanceEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal?>("Balance")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.CashDepositEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("DepositAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("DepositPeriod")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("InterestRate")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CashDeposits");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.CreditEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("CreditAmount")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("CreditPeriod")
                        .HasColumnType("numeric");

                    b.Property<long?>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("InitialPayment")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("InterestRate")
                        .HasColumnType("numeric");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Credits");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.CustomerEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.PaymentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal?>("PaymentAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("RecipientCardNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.BalanceEntity", b =>
                {
                    b.HasOne("dbank.Web.Domain.Entities.CustomerEntity", "Customer")
                        .WithOne("Balance")
                        .HasForeignKey("dbank.Web.Domain.Entities.BalanceEntity", "CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.CashDepositEntity", b =>
                {
                    b.HasOne("dbank.Web.Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("CashDeposits")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.CreditEntity", b =>
                {
                    b.HasOne("dbank.Web.Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("Credits")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.PaymentEntity", b =>
                {
                    b.HasOne("dbank.Web.Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("Payments")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("dbank.Web.Domain.Entities.CustomerEntity", b =>
                {
                    b.Navigation("Balance");

                    b.Navigation("CashDeposits");

                    b.Navigation("Credits");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
