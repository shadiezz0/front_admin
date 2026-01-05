using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitailCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeAttributeType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeAttributeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeTypeCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeAttributeMain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeTypeId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CodeAttributeTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeAttributeMain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeAttributeMain_CodeAttributeType_CodeAttributeTypeId",
                        column: x => x.CodeAttributeTypeId,
                        principalTable: "CodeAttributeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Code",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeTypeId = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CodeGenerated = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "DRAFT"),
                    ExternalSystem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Code", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Code_CodeType_CodeTypeId",
                        column: x => x.CodeTypeId,
                        principalTable: "CodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CodeTypeSequence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodeTypeId = table.Column<int>(type: "int", nullable: false),
                    StartWith = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    MinValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false),
                    CurrentValue = table.Column<int>(type: "int", nullable: false),
                    IsCycling = table.Column<int>(type: "int", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeTypeSequence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeTypeSequence_CodeType_CodeTypeId",
                        column: x => x.CodeTypeId,
                        principalTable: "CodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeAttributeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttributeMainId = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ParentDetailId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeAttributeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeAttributeDetails_CodeAttributeDetails_ParentDetailId",
                        column: x => x.ParentDetailId,
                        principalTable: "CodeAttributeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodeAttributeDetails_CodeAttributeMain_AttributeMainId",
                        column: x => x.AttributeMainId,
                        principalTable: "CodeAttributeMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CodeTypeSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeTypeId = table.Column<int>(type: "int", nullable: false),
                    AttributeDetailId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Separator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "-"),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegUserID = table.Column<int>(type: "int", nullable: true),
                    RegComputerID = table.Column<int>(type: "int", nullable: true),
                    CancelDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelUserID = table.Column<int>(type: "int", nullable: true),
                    CancelComputerID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeTypeSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeTypeSetting_CodeAttributeDetails_AttributeDetailId",
                        column: x => x.AttributeDetailId,
                        principalTable: "CodeAttributeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CodeTypeSetting_CodeType_CodeTypeId",
                        column: x => x.CodeTypeId,
                        principalTable: "CodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Code_CodeTypeId_CodeGenerated",
                table: "Code",
                columns: new[] { "CodeTypeId", "CodeGenerated" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeAttributeDetails_AttributeMainId_Code",
                table: "CodeAttributeDetails",
                columns: new[] { "AttributeMainId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeAttributeDetails_ParentDetailId",
                table: "CodeAttributeDetails",
                column: "ParentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeAttributeMain_CodeAttributeTypeId_Code",
                table: "CodeAttributeMain",
                columns: new[] { "CodeAttributeTypeId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeType_CodeTypeCode",
                table: "CodeType",
                column: "CodeTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodeTypeSequence_CodeTypeId",
                table: "CodeTypeSequence",
                column: "CodeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeTypeSetting_AttributeDetailId",
                table: "CodeTypeSetting",
                column: "AttributeDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeTypeSetting_CodeTypeId_AttributeDetailId",
                table: "CodeTypeSetting",
                columns: new[] { "CodeTypeId", "AttributeDetailId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Code");

            migrationBuilder.DropTable(
                name: "CodeTypeSequence");

            migrationBuilder.DropTable(
                name: "CodeTypeSetting");

            migrationBuilder.DropTable(
                name: "CodeAttributeDetails");

            migrationBuilder.DropTable(
                name: "CodeType");

            migrationBuilder.DropTable(
                name: "CodeAttributeMain");

            migrationBuilder.DropTable(
                name: "CodeAttributeType");
        }
    }
}
