using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Projeto.Alfa12.Migrations
{
    public partial class MóduloUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TurmaId",
                table: "Pontuacoes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "Respondido",
                table: "Pontuacoes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "Arquivo",
                table: "Modulos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxPonto",
                table: "Modulos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Resposta",
                table: "Modulos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Texto",
                table: "Modulos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Modulos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Respondido",
                table: "Pontuacoes");

            migrationBuilder.DropColumn(
                name: "Arquivo",
                table: "Modulos");

            migrationBuilder.DropColumn(
                name: "MaxPonto",
                table: "Modulos");

            migrationBuilder.DropColumn(
                name: "Resposta",
                table: "Modulos");

            migrationBuilder.DropColumn(
                name: "Texto",
                table: "Modulos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Modulos");

            migrationBuilder.AlterColumn<int>(
                name: "TurmaId",
                table: "Pontuacoes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
