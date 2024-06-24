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

namespace OutOfOffice.Pages.LeaveRequest
{
    public class DeleteModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public DeleteModel(
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

        [BindProperty]
        public LeaveRequestModel LeaveRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.LeaveRequestsList))
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }

            var leaverequest = await _context.LeaveRequests.FirstOrDefaultAsync(m => m.ID == id);

            if (leaverequest == null)
            {
                return NotFound();
            }
            else
            {
                LeaveRequest = leaverequest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.LeaveRequestsList))
            {
                return Forbid();
            }
            if (id == null)
            {
                return NotFound();
            }

            var leaverequest = await _context.LeaveRequests.FindAsync(id);
            if (leaverequest != null)
            {
                LeaveRequest = leaverequest;
                _context.LeaveRequests.Remove(LeaveRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
