﻿// <auto-generated />
using GerenciadorDespesas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GerenciadorDespesas.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20190602213704_criacaoDB")]
    partial class criacaoDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GerenciadorDespesas.Models.Despesas", b =>
                {
                    b.Property<int>("DespesaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MesId");

                    b.Property<int>("TipoDespesaId");

                    b.Property<double>("Valor");

                    b.HasKey("DespesaId");

                    b.HasIndex("MesId");

                    b.HasIndex("TipoDespesaId");

                    b.ToTable("Despesas");
                });

            modelBuilder.Entity("GerenciadorDespesas.Models.Meses", b =>
                {
                    b.Property<int>("MesId");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("MesId");

                    b.ToTable("Meses");
                });

            modelBuilder.Entity("GerenciadorDespesas.Models.Salarios", b =>
                {
                    b.Property<int>("SalarioId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MesId");

                    b.Property<double>("Valor");

                    b.HasKey("SalarioId");

                    b.HasIndex("MesId")
                        .IsUnique();

                    b.ToTable("Salarios");
                });

            modelBuilder.Entity("GerenciadorDespesas.Models.TipoDespesas", b =>
                {
                    b.Property<int>("TipoDespesaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("TipoDespesaId");

                    b.ToTable("TipoDespesas");
                });

            modelBuilder.Entity("GerenciadorDespesas.Models.Despesas", b =>
                {
                    b.HasOne("GerenciadorDespesas.Models.Meses", "Meses")
                        .WithMany("Despesas")
                        .HasForeignKey("MesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GerenciadorDespesas.Models.TipoDespesas", "TipoDespesas")
                        .WithMany("Despesas")
                        .HasForeignKey("TipoDespesaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GerenciadorDespesas.Models.Salarios", b =>
                {
                    b.HasOne("GerenciadorDespesas.Models.Meses", "Meses")
                        .WithOne("Salarios")
                        .HasForeignKey("GerenciadorDespesas.Models.Salarios", "MesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
