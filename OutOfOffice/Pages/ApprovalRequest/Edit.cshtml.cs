using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Pages.ApprovalRequest
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

        [ViewData]
        public ApprovalRequestModel ApprovalRequest { get; set; }

        public class RejectInput
        {
            public ApprovalRequestModel Request { get; set; }
            public int RequestId { get; set; }
            [Required]
            public string Comment { get; set; }

        }

        [BindProperty]
        public RejectInput RejectInputModel { get; set; } = default!;


        public async Task<IActionResult> OnGetUpdateAsync(int? id, RequestStatus status)
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
                return NotFound();
            var approvalRequest = await _context.ApprovalRequests.Include(x => x.LeaveRequest).Include(x => x.LeaveRequest.Employee).FirstOrDefaultAsync(a => a.ID == id);
            if (approvalRequest == null)
                return NotFound();

            switch (status)
            {
                case RequestStatus.Approved:
                    approvalRequest.Status = RequestStatus.Approved;
                    approvalRequest.LeaveRequest.Status = RequestStatus.Approved;
                    approvalRequest.LeaveRequest.Employee.Balance -= approvalRequest.LeaveRequest.EndDate.DayNumber - approvalRequest.LeaveRequest.StartDate.DayNumber;
                    break;
                case RequestStatus.Rejected:
                    ApprovalRequest = approvalRequest;
                    return Page();
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostUpdateAsync()
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
            var approvalRequest = await _context.ApprovalRequests.Include(x => x.LeaveRequest).FirstOrDefaultAsync(a => a.ID == RejectInputModel.RequestId);

            approvalRequest.Comment = RejectInputModel?.Comment;
            approvalRequest.Status = RequestStatus.Rejected;
            approvalRequest.LeaveRequest.Status = RequestStatus.Rejected;


            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
