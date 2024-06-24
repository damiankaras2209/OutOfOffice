using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.LeaveRequest
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
        public string EmployeeSort { get; set; }
        public string AbsenceReasonSort { get; set; }
        public string StartDateSort { get; set; }
        public string EndDateSort { get; set; }
        public string CommentSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentSort { get; set; }


        public string IdSearch { get; set; }
        public string EmployeeSearch { get; set; }

        public IList<LeaveRequestModel> LeaveRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string sortOrder, string idSearch, string employeeSearch)
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

            CurrentSort = sortOrder;
            IDSort = sortOrder == "id" ? "id_desc" : "id";
            EmployeeSort = sortOrder == "employee" ? "employee_desc" : "employee";
            AbsenceReasonSort = sortOrder == "reason" ? "reason_desc" : "reason";
            StartDateSort = sortOrder == "start" ? "start_desc" : "start";
            EndDateSort = sortOrder == "end" ? "end_desc" : "end";
            CommentSort = sortOrder == "comment" ? "comment_desc" : "comment";
            StatusSort = sortOrder == "status" ? "status_desc" : "status";

            IdSearch = idSearch;
            EmployeeSearch = employeeSearch;

            IQueryable<LeaveRequestModel> leaveRequestIQ = from l in _context.LeaveRequests.Include(l => l.Employee)
                                                           select l;

            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.LeaveRequestsFullList))
            {

                leaveRequestIQ = leaveRequestIQ.Where(l => l.Employee == CurrentUser);
            }

            //ProjectTypes = new SelectList((ProjectType[])Enum.GetValues(typeof(ProjectType)));

            if (!String.IsNullOrEmpty(idSearch))
                leaveRequestIQ = leaveRequestIQ.Where(l => l.ID.ToString() == idSearch);
            if (!String.IsNullOrEmpty(employeeSearch))
                leaveRequestIQ = leaveRequestIQ.Where(l => l.Employee.FullName.Contains(employeeSearch));

            switch (sortOrder)
            {
                case "id": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.ID); break;
                case "id_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.ID); break;
                case "employee": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.Employee.FullName); break;
                case "employee_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.Employee.FullName); break;
                case "reason": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.AbsenceReason); break;
                case "reason_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.AbsenceReason); break;
                case "start": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.StartDate); break;
                case "start_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.StartDate); break;
                case "end": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.EndDate); break;
                case "end_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.EndDate); break;
                case "comment": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.Comment); break;
                case "comment_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.Comment); break;
                case "status": leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.Status); break;
                case "status_desc": leaveRequestIQ = leaveRequestIQ.OrderByDescending(l => l.Status); break;
                default:
                    leaveRequestIQ = leaveRequestIQ.OrderBy(l => l.ID);
                    break;
            }


            LeaveRequest = await leaveRequestIQ.AsNoTracking().ToListAsync();
            return Page();
        }
    }
}
