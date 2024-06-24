using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.Employee
{
    public class AssignModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public AssignModel(
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
        public EmployeeModel Employee { get; set; }
        [ViewData]
        public SelectList Projects { get; set; }

        public class AssignProjectModel
        {
            public string Id { get; set; }
            public ProjectModel Project { get; set; }
            public int ProjectId { get; set; }
        }

        [BindProperty]
        public AssignProjectModel AssignProject { get; set; } = default!;

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

            Employee = _context.Employees.Where(e => e.Id == id).FirstOrDefault();

            var projects = await _context.Projects.Include(p => p.ProjectManager).Select(p => new { ID = p.ID, Label = p.ID + "; " + p.ProjectType + "; " + p.StartDate.ToShortDateString() + " - " + p.EndDate.ToShortDateString() + "; employee: " + p.ProjectManager.FullName }).ToListAsync();

            Projects = new SelectList(projects, "ID", "Label");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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
            EmployeeModel employee = _context.Employees.Where(e => e.Id == AssignProject.Id).FirstOrDefault();
            ProjectModel project = _context.Projects.Where(e => e.ID == AssignProject.ProjectId).FirstOrDefault();

            employee.Project = project;
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

    }
}
