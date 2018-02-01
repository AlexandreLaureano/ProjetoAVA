using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Projeto.Alfa12.Migrations
{
    public partial class att3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Turmas_TurmaId",
                table: "Turmas");

            migrationBuilder.DropIndex(
                name: "IX_Turmas_TurmaId",
                table: "Turmas");

            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "Turmas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurmaId",
                table: "Turmas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_TurmaId",
                table: "Turmas",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turmas_Turmas_TurmaId",
                table: "Turmas",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
