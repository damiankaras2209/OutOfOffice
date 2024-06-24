using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutOfOffice.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessResources",
                columns: table => new
                {
                    Resource = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    HasAccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessResources", x => new { x.Position, x.Resource });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeaveRequestID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRequests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subdivision = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PeoplePartnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_PeoplePartnerId",
                        column: x => x.PeoplePartnerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AbsenceReason = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProjectManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Projects_Employees_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccessResources",
                columns: new[] { "Position", "Resource", "HasAccess" },
                values: new object[,]
                {
                    { 0, 0, true },
                    { 0, 1, true },
                    { 0, 2, true },
                    { 0, 3, true },
                    { 0, 4, false },
                    { 0, 5, true },
                    { 0, 6, true },
                    { 0, 7, true },
                    { 0, 8, true },
                    { 0, 9, false },
                    { 1, 0, true },
                    { 1, 1, false },
                    { 1, 2, true },
                    { 1, 3, true },
                    { 1, 4, false },
                    { 1, 5, true },
                    { 1, 6, true },
                    { 1, 7, false },
                    { 1, 8, true },
                    { 1, 9, true },
                    { 2, 0, true },
                    { 2, 1, true },
                    { 2, 2, true },
                    { 2, 3, true },
                    { 2, 4, true },
                    { 2, 5, true },
                    { 2, 6, true },
                    { 2, 7, true },
                    { 2, 8, true },
                    { 2, 9, false },
                    { 3, 0, false },
                    { 3, 1, false },
                    { 3, 2, true },
                    { 3, 3, false },
                    { 3, 4, true },
                    { 3, 5, false },
                    { 3, 6, true },
                    { 3, 7, false },
                    { 3, 8, false },
                    { 3, 9, false }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Balance", "FullName", "NID", "NormalizedUserName", "PasswordHash", "PeoplePartnerId", "Position", "ProjectId", "Status", "Subdivision", "UserName" },
                values: new object[,]
                {
                    { "7efca1d3-da46-4110-a66d-254887f88111", 0, "HR Manager", 1, "hrmanager", "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==", "7efca1d3-da46-4110-a66d-254887f88111", 1, null, 0, 0, "hrmanager" },
                    { "070e9568-8359-47dc-ae39-2cd59304c7bb", 14, "Employee 2", 6, "employee2", "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==", "7efca1d3-da46-4110-a66d-254887f88111", 3, null, 0, 0, "employee2" },
                    { "15eba506-1421-45bd-a2f3-900e73899ccf", 0, "Project Manager", 3, "projectmanager", "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==", "7efca1d3-da46-4110-a66d-254887f88111", 0, null, 0, 0, "projectmanager" },
                    { "6d2e2a5d-140a-4b51-9155-94c879a67fbd", 20, "Employee", 5, "employee", "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==", "7efca1d3-da46-4110-a66d-254887f88111", 3, null, 0, 0, "employee" },
                    { "f127b9c0-c89a-4695-a610-1747f1e188e0", 0, "Administrator", 2, "administrator", "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==", "7efca1d3-da46-4110-a66d-254887f88111", 2, null, 0, 0, "administrator" },
                    { "f777607e-284c-47b8-a16b-969feb867b0f", 0, "PM", 4, "projectmanager2", "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==", "7efca1d3-da46-4110-a66d-254887f88111", 0, null, 0, 1, "projectmanager2" }
                });

            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "ID", "AbsenceReason", "Comment", "EmployeeId", "EndDate", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 2, "Comment A", "6d2e2a5d-140a-4b51-9155-94c879a67fbd", new DateOnly(2024, 6, 3), new DateOnly(2024, 6, 1), 0 },
                    { 2, 0, "Comment B", "070e9568-8359-47dc-ae39-2cd59304c7bb", new DateOnly(2024, 6, 23), new DateOnly(2024, 6, 23), 0 },
                    { 3, 3, "Comment C", "6d2e2a5d-140a-4b51-9155-94c879a67fbd", new DateOnly(2024, 3, 30), new DateOnly(2024, 3, 20), 0 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ID", "Comment", "EndDate", "ProjectManagerId", "ProjectType", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, "Comment X", new DateOnly(2024, 11, 30), "15eba506-1421-45bd-a2f3-900e73899ccf", 2, new DateOnly(2023, 10, 1), 0 },
                    { 2, "Comment Z", new DateOnly(2024, 5, 30), "15eba506-1421-45bd-a2f3-900e73899ccf", 0, new DateOnly(2024, 1, 15), 1 },
                    { 3, "Comment Y", new DateOnly(2023, 12, 31), "15eba506-1421-45bd-a2f3-900e73899ccf", 1, new DateOnly(2020, 1, 1), 0 }
                });

            migrationBuilder.InsertData(
                table: "ApprovalRequests",
                columns: new[] { "ID", "ApproverId", "Comment", "LeaveRequestID", "Status" },
                values: new object[,]
                {
                    { 1, "7efca1d3-da46-4110-a66d-254887f88111", null, 1, 0 },
                    { 2, "7efca1d3-da46-4110-a66d-254887f88111", null, 2, 0 },
                    { 3, "7efca1d3-da46-4110-a66d-254887f88111", null, 3, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_ApproverId",
                table: "ApprovalRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_LeaveRequestID",
                table: "ApprovalRequests",
                column: "LeaveRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PeoplePartnerId",
                table: "Employees",
                column: "PeoplePartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProjectId",
                table: "Employees",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Employees",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRequests_Employees_ApproverId",
                table: "ApprovalRequests",
                column: "ApproverId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRequests_LeaveRequests_LeaveRequestID",
                table: "ApprovalRequests",
                column: "LeaveRequestID",
                principalTable: "LeaveRequests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Employees_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Employees_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Employees_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Employees_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Projects_ProjectId",
                table: "Employees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "AccessResources");

            migrationBuilder.DropTable(
                name: "ApprovalRequests");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
