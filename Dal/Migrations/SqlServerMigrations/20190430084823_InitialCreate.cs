using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LogoPaasSampleApp.Dal.Migrations.SqlServerMigrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EMAIL = table.Column<string>(nullable: true),
                    INSERTDATE = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2019, 4, 30, 11, 48, 23, 0, DateTimeKind.Unspecified)),
                    INSERTEDBY = table.Column<long>(nullable: false),
                    ISACTIVE = table.Column<bool>(nullable: false),
                    LASTUPDATEDATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    LASTUPDATEDBY = table.Column<long>(nullable: true),
                    NAME = table.Column<string>(nullable: true),
                    SURNAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
