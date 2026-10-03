using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventario.Celulares.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigracionMySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inv_locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inv_locations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inv_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inv_users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inv_devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Brand = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Imei = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Observations = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchasePrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inv_devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inv_devices_inv_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "inv_locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "inv_movements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    DeviceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FromLocationId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ToLocationId = table.Column<Guid>(type: "char(36)", nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisteredById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inv_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inv_movements_inv_devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "inv_devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inv_movements_inv_locations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "inv_locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inv_movements_inv_locations_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "inv_locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_devices_brand",
                table: "inv_devices",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "ix_devices_status",
                table: "inv_devices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_inv_devices_LocationId",
                table: "inv_devices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "ux_devices_imei",
                table: "inv_devices",
                column: "Imei",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_locations_code",
                table: "inv_locations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inv_movements_FromLocationId",
                table: "inv_movements",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_inv_movements_ToLocationId",
                table: "inv_movements",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "ix_movements_device_id",
                table: "inv_movements",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "ix_movements_occurred_at",
                table: "inv_movements",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "ux_users_username",
                table: "inv_users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inv_movements");

            migrationBuilder.DropTable(
                name: "inv_users");

            migrationBuilder.DropTable(
                name: "inv_devices");

            migrationBuilder.DropTable(
                name: "inv_locations");
        }
    }
}
