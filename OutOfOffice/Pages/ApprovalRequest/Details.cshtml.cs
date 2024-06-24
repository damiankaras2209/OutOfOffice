using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Data;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.ApprovalRequest
{
    public class DetailsModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public DetailsModel (
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

        public ApprovalRequestModel ApprovalRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.ApprovalRequestsList))
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }

            var approvalrequest = await _context.ApprovalRequests.Include(a => a.Approver).Include(a => a.LeaveRequest).Include(a => a.LeaveRequest.Employee).FirstOrDefaultAsync(m => m.ID == id);
            if (approvalrequest == null)
            {
                return NotFound();
            }
            else
            {
                ApprovalRequest = approvalrequest;
            }
            return Page();
        }
    }
}
