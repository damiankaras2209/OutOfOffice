using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Data;
using OutOfOffice.Models;
using OutOfOffice.Helpers;
using Microsoft.AspNetCore.Identity;

namespace OutOfOffice.Pages.LeaveRequest
{
    public class EditModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public EditModel(
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
                return NotFound();
            var leaverequest =  await _context.LeaveRequests.FirstOrDefaultAsync(m => m.ID == id);
            if (leaverequest == null)
                return NotFound();

            LeaveRequest = leaverequest;
            return Page();
        }

        public async Task<IActionResult> OnGetUpdateAsync(int? id, RequestStatus status)
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
                return NotFound();
            var leaverequest = await _context.LeaveRequests.Include(x=>x.Employee).Include(x=>x.Employee.PeoplePartner).FirstOrDefaultAsync(m => m.ID == id);
            if (leaverequest == null)
                return NotFound();

            switch (status)
            {
                case RequestStatus.Submitted:
                    var approval = new ApprovalRequestModel();
                    approval.Approver = leaverequest.Employee.PeoplePartner;
                    approval.LeaveRequest = leaverequest;
                    approval.Status = RequestStatus.New;
                    await _context.ApprovalRequests.AddAsync(approval);
                    break;
                case RequestStatus.Cancelled:
                    var approval2 = await _context.ApprovalRequests.Where(x => x.LeaveRequest == leaverequest).FirstOrDefaultAsync();
                    if (approval2 == null) break;
                    approval2.Status = RequestStatus.Cancelled;
                    break;
            }

            leaverequest.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LeaveRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveRequestExists(LeaveRequest.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(e => e.ID == id);
        }
    }
}
