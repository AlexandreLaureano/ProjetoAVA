﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;
using System;

namespace Projeto.Alfa12.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180623164334_dtresposta")]
    partial class dtresposta
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("ProviderKey");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("ProviderKey", "LoginProvider");

                    b.HasAlternateKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("RoleId", "UserId");

                    b.HasAlternateKey("UserId", "RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.HasAlternateKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.AlunoConquista", b =>
                {
                    b.Property<int>("AlunoId");

                    b.Property<int>("ConquistaId");

                    b.HasKey("AlunoId", "ConquistaId");

                    b.HasIndex("ConquistaId");

                    b.ToTable("AlunoConquistas");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.AlunoTurma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlunoId");

                    b.Property<bool>("Ativo");

                    b.Property<DateTime>("DataIngressao");

                    b.Property<int>("TurmaId");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

                    b.HasIndex("TurmaId");

                    b.ToTable("AlunoTurmas");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Ativo");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DataNascimento");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Sobrenome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Conquista", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome");

                    b.Property<string>("Requisito");

                    b.HasKey("Id");

                    b.ToTable("Conquistas");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.LogUsuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Acao");

                    b.Property<DateTime>("Data");

                    b.Property<int>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Mensagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Data");

                    b.Property<int>("DestinoFK");

                    b.Property<int>("OrigemFK");

                    b.Property<string>("Texto");

                    b.HasKey("Id");

                    b.HasIndex("DestinoFK");

                    b.HasIndex("OrigemFK");

                    b.ToTable("Mensagens");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Modulo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Arquivo");

                    b.Property<DateTime>("DataMax");

                    b.Property<string>("Descricao");

                    b.Property<int>("MaxPonto");

                    b.Property<string>("Nome");

                    b.Property<bool>("Respondido");

                    b.Property<string>("Resposta");

                    b.Property<string>("Texto");

                    b.Property<int>("Tipo");

                    b.Property<int?>("TurmaId");

                    b.Property<string>("Url");

                    b.Property<bool>("Visivel");

                    b.HasKey("Id");

                    b.HasIndex("TurmaId");

                    b.ToTable("Modulos");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Pontuacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlunoId");

                    b.Property<DateTime>("Data");

                    b.Property<int>("ModuloId");

                    b.Property<int>("Pontos");

                    b.Property<bool>("Respondido");

                    b.Property<string>("Resposta");

                    b.Property<int?>("TurmaId");

                    b.HasKey("Id");

                    b.HasIndex("AlunoId");

                    b.HasIndex("ModuloId");

                    b.ToTable("Pontuacoes");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Turma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AreaConhecimento");

                    b.Property<string>("ChaveAcesso");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<string>("Descricao");

                    b.Property<int?>("MaxPonto");

                    b.Property<string>("Nome");

                    b.Property<int?>("ProfessorId");

                    b.HasKey("Id");

                    b.HasIndex("ProfessorId");

                    b.ToTable("Turmas");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Aluno", b =>
                {
                    b.HasBaseType("Projeto.Alfa12.Models.ApplicationUser");

                    b.Property<int?>("PontoGeral");

                    b.Property<int>("Ra");

                    b.ToTable("Aluno");

                    b.HasDiscriminator().HasValue("Aluno");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Coordenador", b =>
                {
                    b.HasBaseType("Projeto.Alfa12.Models.ApplicationUser");


                    b.ToTable("Coordenador");

                    b.HasDiscriminator().HasValue("Coordenador");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Professor", b =>
                {
                    b.HasBaseType("Projeto.Alfa12.Models.ApplicationUser");

                    b.Property<bool>("Autenticado");

                    b.ToTable("Professor");

                    b.HasDiscriminator().HasValue("Professor");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.AlunoConquista", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.Aluno", "Aluno")
                        .WithMany("Conquistas")
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Projeto.Alfa12.Models.Conquista", "Conquista")
                        .WithMany("Alunos")
                        .HasForeignKey("ConquistaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.AlunoTurma", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.Aluno", "Aluno")
                        .WithMany("Turmas")
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Projeto.Alfa12.Models.Turma", "Turma")
                        .WithMany("Alunos")
                        .HasForeignKey("TurmaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.LogUsuario", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser", "Usuario")
                        .WithMany("Logs")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Mensagem", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser", "Destino")
                        .WithMany("Destino")
                        .HasForeignKey("DestinoFK")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Projeto.Alfa12.Models.ApplicationUser", "Origem")
                        .WithMany("Origem")
                        .HasForeignKey("OrigemFK")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Modulo", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.Turma", "Turma")
                        .WithMany("Modulos")
                        .HasForeignKey("TurmaId");
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Pontuacao", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.Aluno", "Aluno")
                        .WithMany("Pontuacao")
                        .HasForeignKey("AlunoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Projeto.Alfa12.Models.Modulo", "Modulo")
                        .WithMany("Pontuacao")
                        .HasForeignKey("ModuloId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Projeto.Alfa12.Models.Turma", b =>
                {
                    b.HasOne("Projeto.Alfa12.Models.Professor", "Professor")
                        .WithMany("Turmas")
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
