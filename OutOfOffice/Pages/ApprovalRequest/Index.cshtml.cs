using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Data;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.ApprovalRequest
{
    public class IndexModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public IndexModel(
            OutOfOffice.Data.ApplicationDbContext context,
            UserManager<EmployeeModel> userManager,
            Access access
        )
        {
            _context = context;
            _userManager = userManager;
            _access = access;
        }

        [ViewData]
        public EmployeeModel? CurrentUser { get; set; }

        public string IDSort { get; set; }
        public string ApproverSort { get; set; }
        public string LeaveRequestSort { get; set; }
        public string EmployeeSort { get; set; }
        public string StatusSort { get; set; }
        public string CommentSort { get; set; }
        public string CurrentSort { get; set; }

        public string IdSearch { get; set; }
        public string ApproverFilter { get; set; }
        public string EmployeeFilter { get; set; }

        public IList<ApprovalRequestModel> ApprovalRequest { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(string sortOrder, string idSearch, string approverSearch, string employeeSearch)
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (! _access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.ApprovalRequestsList))
            {
                return Forbid();
            }

            CurrentSort = sortOrder;
            IDSort = sortOrder == "id" ? "id_desc" : "id";
            ApproverSort = sortOrder == "approver" ? "approver_desc" : "approver";
            LeaveRequestSort = sortOrder == "lr" ? "lr_desc" : "lr";
            EmployeeSort = sortOrder == "employee" ? "employee_desc" : "employee";
            StatusSort = sortOrder == "status" ? "status_desc" : "status";
            CommentSort = sortOrder == "comment" ? "comment_desc" : "comment";

            IdSearch = idSearch;
            ApproverFilter = approverSearch;
            EmployeeFilter = employeeSearch;

            IQueryable<ApprovalRequestModel> approverRequestIQ = from a in _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).Include(a => a.LeaveRequest.Employee)
                                                                 select a;


            if (!String.IsNullOrEmpty(idSearch))
                approverRequestIQ = approverRequestIQ.Where(a => a.ID.ToString() == idSearch);
            if (!String.IsNullOrEmpty(approverSearch))
                approverRequestIQ = approverRequestIQ.Where(a => a.Approver.FullName.Contains(approverSearch));
            if (!String.IsNullOrEmpty(employeeSearch))
                approverRequestIQ = approverRequestIQ.Where(a => a.LeaveRequest.Employee.FullName.Contains(employeeSearch));

            switch (sortOrder)
            {
                case "id":              approverRequestIQ = approverRequestIQ.OrderBy(a => a.ID); break;
                case "id_desc":         approverRequestIQ = approverRequestIQ.OrderByDescending(p => p.ID); break;
                case "approver":        approverRequestIQ = approverRequestIQ.OrderBy(a => a.Approver.FullName); break;
                case "approver_desc":   approverRequestIQ = approverRequestIQ.OrderByDescending(p => p.Approver.FullName); break;
                case "lr":              approverRequestIQ = approverRequestIQ.OrderBy(a => a.LeaveRequest.ID); break;
                case "lr_desc":         approverRequestIQ = approverRequestIQ.OrderByDescending(p => p.LeaveRequest.ID); break;
                case "employee":        approverRequestIQ = approverRequestIQ.OrderBy(a => a.LeaveRequest.Employee.FullName); break;
                case "employee_desc":   approverRequestIQ = approverRequestIQ.OrderByDescending(p => p.LeaveRequest.Employee.FullName); break;
                case "comment":         approverRequestIQ = approverRequestIQ.OrderBy(a => a.Comment); break;
                case "comment_desc":    approverRequestIQ = approverRequestIQ.OrderByDescending(p => p.Comment); break;
                case "status":          approverRequestIQ = approverRequestIQ.OrderBy(a => a.Status); break;
                case "status_desc":     approverRequestIQ = approverRequestIQ.OrderByDescending(p => p.Status); break;
                default:
                    approverRequestIQ = approverRequestIQ.OrderBy(p => p.ID);
                    break;
            }

            ApprovalRequest = await approverRequestIQ.AsNoTracking().ToListAsync();
            return Page();
        }
    }
}
