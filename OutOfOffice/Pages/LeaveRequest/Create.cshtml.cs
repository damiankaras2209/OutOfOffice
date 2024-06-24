using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOffice.Helpers;
using OutOfOffice.Models;
using System.Security.Claims;

namespace OutOfOffice.Pages.LeaveRequest
{
    public class CreateModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public CreateModel(
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
        public SelectList Reasons { get; set; }
        [ViewData]
        public SelectList Employees { get; set; }
        [ViewData]
        public SelectList Statuses { get; set; }

        public void PopulateViewData()
        {
            Reasons = new SelectList((AbsenceReason[])Enum.GetValues(typeof(AbsenceReason)));
            Statuses = new SelectList((RequestStatus[])Enum.GetValues(typeof(RequestStatus)));

            var user = _context.Employees.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
            Employees = new SelectList(user, "Id", "FullName");

        }


        public async Task<IActionResult> OnGetAsync()
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
            PopulateViewData();
            return Page();
        }

        [BindProperty]
        public LeaveRequestModel LeaveRequest { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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

            LeaveRequest.Status = RequestStatus.New;
            _context.LeaveRequests.Add(LeaveRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
