using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;
using System;

namespace OutOfOffice.Data
{
    public class ApplicationDbContext : IdentityDbContext<EmployeeModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<ApprovalRequestModel> ApprovalRequests { get; set; }
        public DbSet<LeaveRequestModel> LeaveRequests { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<AccessResourceModel> AccessResources { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EmployeeModel>().ToTable("Employees");

            builder.Entity<EmployeeModel>()
                //.Ignore(c => c.NormalizedUserName)
                .Ignore(c => c.Email)
                .Ignore(c => c.NormalizedEmail)
                .Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.SecurityStamp)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.AccessFailedCount)
                .Ignore(c => c.ConcurrencyStamp)
                .Ignore(c => c.PhoneNumber);

            builder.Entity<EmployeeModel>()
                .HasOne(e => e.PeoplePartner)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApprovalRequestModel>()
                .HasOne(e => e.LeaveRequest)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AccessResourceModel>()
                .HasKey(e => new { e.Position, e.Resource });

            var hr = Guid.NewGuid().ToString();
            var pm = Guid.NewGuid().ToString();
            var pm2 = Guid.NewGuid().ToString();
            var e1 = Guid.NewGuid().ToString();
            var e2 = Guid.NewGuid().ToString();
            var nid = 1;
            builder.Entity<EmployeeModel>()
                .HasData(new {
                    Id = hr,
                    NID = nid++,
                    UserName = "hrmanager",
                    NormalizedUserName = "hrmanager",
                    PasswordHash = "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==",
                    AccessFailedCount = 0,
                    FullName = "HR Manager",
                    Balance = 0,
                    PeoplePartnerId = hr,
                    Position = Position.HRManager,
                    Subdivision = Subdivision.Subdivision1,
                    Status = Status.Active
                });
            builder.Entity<EmployeeModel>()
                .HasData(new {
                    Id = Guid.NewGuid().ToString(),
                    NID = nid++,
                    UserName = "administrator",
                    NormalizedUserName = "administrator",
                    PasswordHash = "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==",
                    AccessFailedCount = 0,
                    FullName = "Administrator",
                    Balance = 0,
                    PeoplePartnerId = hr,
                    Position = Position.Administrator,
                    Subdivision = Subdivision.Subdivision1,
                    Status = Status.Active
                });
            builder.Entity<EmployeeModel>().HasData(new
                {
                    Id = pm,
                    NID = nid++,
                    UserName = "projectmanager",
                    NormalizedUserName = "projectmanager",
                    PasswordHash = "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==",
                    AccessFailedCount = 0,
                    FullName = "Project Manager",
                    Balance = 0,
                    PeoplePartnerId = hr,
                    Position = Position.ProjectManager,
                    Subdivision = Subdivision.Subdivision1,
                    Status = Status.Active
                });
            builder.Entity<EmployeeModel>().HasData(new
                {
                    Id = pm2,
                    NID = nid++,
                    UserName = "projectmanager2",
                    NormalizedUserName = "projectmanager2",
                    PasswordHash = "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==",
                    AccessFailedCount = 0,
                    FullName = "PM",
                    Balance = 0,
                    PeoplePartnerId = hr,
                    Position = Position.ProjectManager,
                    Subdivision = Subdivision.Subdivision2,
                    Status = Status.Active
                });
            builder.Entity<EmployeeModel>().HasData(new
                {
                    Id = e1,
                    NID = nid++,
                    UserName = "employee",
                    NormalizedUserName = "employee",
                    PasswordHash = "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==",
                    AccessFailedCount = 0,
                    FullName = "Employee",
                    Balance = 20,
                    PeoplePartnerId = hr,
                    Position = Position.Employee,
                    Subdivision = Subdivision.Subdivision1,
                    Status = Status.Active
                });
            builder.Entity<EmployeeModel>().HasData(new
                {
                    Id = e2,
                    NID = nid++,
                    UserName = "employee2",
                    NormalizedUserName = "employee2",
                    PasswordHash = "AQAAAAIAAYagAAAAEJf3mcMMgf5yHMLgut6WGHsTbXx5Ha6dTYl6rkCXE11KQMs1FuJ6W7q6pNbTLuONBg==",
                    AccessFailedCount = 0,
                    FullName = "Employee 2",
                    Balance = 14,
                    PeoplePartnerId = hr,
                    Position = Position.Employee,
                    Subdivision = Subdivision.Subdivision1,
                    Status = Status.Active
                });



            builder.Entity<ProjectModel>().HasData(new
            {
                ID = 1,
                ProjectType = ProjectType.TYPE3,
                StartDate = new DateOnly(2023,10,1),
                EndDate = new DateOnly(2024,11,30),
                ProjectManagerId = pm,
                Status = Status.Active,
                Comment = "Comment X"
            });
            builder.Entity<ProjectModel>().HasData(new
            {
                ID = 2,
                ProjectType = ProjectType.TYPE1,
                StartDate = new DateOnly(2024, 01, 15),
                EndDate = new DateOnly(2024, 05, 30),
                ProjectManagerId = pm,
                Status = Status.Inactive,
                Comment = "Comment Z"
            });
            builder.Entity<ProjectModel>().HasData(new
            {
                ID = 3,
                ProjectType = ProjectType.TYPE2,
                StartDate = new DateOnly(2020, 1, 1),
                EndDate = new DateOnly(2023, 12, 31),
                ProjectManagerId = pm,
                Status = Status.Active,
                Comment = "Comment Y"
            });



            builder.Entity<LeaveRequestModel>().HasData(new
            {
                ID = 1,
                EmployeeId = e1,
                AbsenceReason = AbsenceReason.Reason3,
                StartDate = new DateOnly(2024, 6, 1),
                EndDate = new DateOnly(2024, 6, 3),
                Status = RequestStatus.New,
                Comment = "Comment A"
            });
            builder.Entity<LeaveRequestModel>().HasData(new
            {
                ID = 2,
                EmployeeId = e2,
                AbsenceReason = AbsenceReason.Reason1,
                StartDate = new DateOnly(2024, 6, 23),
                EndDate = new DateOnly(2024, 6, 23),
                Status = RequestStatus.New,
                Comment = "Comment B"
            });
            builder.Entity<LeaveRequestModel>().HasData(new
            {
                ID = 3,
                EmployeeId = e1,
                AbsenceReason = AbsenceReason.Reason4,
                StartDate = new DateOnly(2024, 3, 20),
                EndDate = new DateOnly(2024, 3, 30),
                Status = RequestStatus.New,
                Comment = "Comment C"
            });

            builder.Entity<ApprovalRequestModel>().HasData(new
            {
                ID = 1,
                ApproverId = hr,
                LeaveRequestID = 1,
                Status = RequestStatus.New,
            });

            builder.Entity<ApprovalRequestModel>().HasData(new
            {
                ID = 2,
                ApproverId = hr,
                LeaveRequestID = 2,
                Status = RequestStatus.New,
            });

            builder.Entity<ApprovalRequestModel>().HasData(new
            {
                ID = 3,
                ApproverId = hr,
                LeaveRequestID = 3,
                Status = RequestStatus.New,
            });

            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)0, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)1, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)2, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)3, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)4, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)5, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)6, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)7, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)0, Resource = (AccessResourceModel.AccessResource)8, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)0, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)1, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)2, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)3, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)4, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)5, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)6, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)7, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)1, Resource = (AccessResourceModel.AccessResource)8, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)0, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)1, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)2, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)3, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)4, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)5, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)6, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)7, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)2, Resource = (AccessResourceModel.AccessResource)8, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)0, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)1, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)2, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)3, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)4, HasAccess = false });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)5, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)6, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)7, HasAccess = true });
            builder.Entity<AccessResourceModel>().HasData(new { Position = (Position)3, Resource = (AccessResourceModel.AccessResource)8, HasAccess = true });


        }

    }

}
