using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.Employee
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
        public string FullNameSort { get; set; }
        public string SubdivisionSort { get; set; }
        public string PositionSort { get; set; }
        public string StatusSort { get; set; }
        public string PartnerSort { get; set; }
        public string BalanceSort { get; set; }
        public string ProjectSort { get; set; }
        public string CurrentSort { get; set; }


        public string IdSearch { get; set; }
        public string FullNameSearch { get; set; }

        public IList<EmployeeModel> Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string sortOrder, string idSearch, string fullNameSearch)
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.EmployeesList))
            {
                return Forbid();
            }

            CurrentSort = sortOrder;
            IDSort = sortOrder == "id" ? "id_desc" : "id";
            FullNameSort = sortOrder == "fullname" ? "fullname_desc" : "fullname";
            SubdivisionSort = sortOrder == "subdivision" ? "subdivision_desc" : "subdivision";
            PositionSort = sortOrder == "position" ? "position_desc" : "position";
            StatusSort = sortOrder == "status" ? "status_desc" : "status";
            PartnerSort = sortOrder == "partner" ? "partner_desc" : "partner";
            BalanceSort = sortOrder == "balance" ? "balance_desc" : "balance";
            ProjectSort = sortOrder == "project" ? "project_desc" : "project";

            IdSearch = idSearch;
            FullNameSearch = fullNameSearch;

            IQueryable<EmployeeModel> employeeIQ = from e in _context.Employees.Include(e => e.PeoplePartner).Include(e => e.Project)
                                                   select e;

            //ProjectTypes = new SelectList((ProjectType[])Enum.GetValues(typeof(ProjectType)));

            if (!String.IsNullOrEmpty(idSearch))
                employeeIQ = employeeIQ.Where(e => e.NID.ToString() == idSearch);
            if (!String.IsNullOrEmpty(fullNameSearch))
                employeeIQ = employeeIQ.Where(e => e.FullName.Contains(fullNameSearch));

            switch (sortOrder)
            {
                case "id": employeeIQ = employeeIQ.OrderBy(e => e.NID); break;
                case "id_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.NID); break;
                case "fullname": employeeIQ = employeeIQ.OrderBy(e => e.FullName); break;
                case "fullname_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.FullName); break;
                case "subdivision": employeeIQ = employeeIQ.OrderBy(e => e.Subdivision); break;
                case "subdivision_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.Subdivision); break;
                case "position": employeeIQ = employeeIQ.OrderBy(e => e.Position); break;
                case "position_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.Position); break;
                case "status": employeeIQ = employeeIQ.OrderBy(e => e.Status); break;
                case "status_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.Status); break;
                case "partner": employeeIQ = employeeIQ.OrderBy(e => e.PeoplePartner.FullName); break;
                case "partner_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.PeoplePartner.FullName); break;
                case "balance": employeeIQ = employeeIQ.OrderBy(e => e.Balance); break;
                case "balance_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.Balance); break;
                case "project": employeeIQ = employeeIQ.OrderBy(e => e.Project.ID); break;
                case "project_desc": employeeIQ = employeeIQ.OrderByDescending(e => e.Project.ID); break;
                default:
                    employeeIQ = employeeIQ.OrderBy(e => e.NID);
                    break;
            }

            Employee = await employeeIQ.AsNoTracking().ToListAsync();
            return Page();
        }
    }
}
