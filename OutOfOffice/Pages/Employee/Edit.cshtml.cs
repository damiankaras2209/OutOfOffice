using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.Employee
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
        public SelectList Subdivisions { get; set; }
        [ViewData]
        public SelectList Positions { get; set; }
        [ViewData]
        public SelectList Partners { get; set; }
        [ViewData]
        public SelectList Statuses { get; set; }

        public void PopulateViewData()
        {
            Subdivisions = new SelectList((Subdivision[])Enum.GetValues(typeof(Subdivision)));
            Positions = new SelectList((Position[])Enum.GetValues(typeof(Position)));
            Partners = new SelectList(_context.Employees.Where(x => x.Position == Position.HRManager), "Id", "FullName");
            Statuses = new SelectList((Status[])Enum.GetValues(typeof(Status)));
        }

        [BindProperty]
        public EmployeeModel EmployeeModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
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
            if (id == null)
                return NotFound();
            var employee = await _context.Employees.Include(e => e.PeoplePartner).FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
                return NotFound();

            PopulateViewData();
            EmployeeModel = employee;
            return Page();
        }

        public async Task<IActionResult> OnGetDeactivateAsync(string? id)
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
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
                return NotFound();

            employee.Status = Status.Inactive;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetActivateAsync(string? id)
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
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
                return NotFound();

            employee.Status = Status.Active;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
