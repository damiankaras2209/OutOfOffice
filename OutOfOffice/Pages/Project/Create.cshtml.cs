using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.Project
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
        public SelectList ProjectTypes { get; set; }
        [ViewData]
        public SelectList ProjectManagers { get; set; }
        [ViewData]
        public SelectList Statuses { get; set; }

        public void PopulateViewData()
        {
            ProjectTypes = new SelectList((ProjectType[])Enum.GetValues(typeof(ProjectType)));
            Statuses = new SelectList((Status[])Enum.GetValues(typeof(Status)));
            ProjectManagers = new SelectList(_context.Employees.Where(x => x.Position == Position.ProjectManager), "Id", "FullName");
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.ProjectsList))
            {
                return Forbid();
            }
            PopulateViewData();
            return Page();
        }

        [BindProperty]
        public ProjectModel ProjectModel { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User?.Identity?.Name;
            if (userName != null)
            {
                CurrentUser = await _userManager.FindByNameAsync(userName);
            }
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.ProjectsList))
            {
                return Forbid();
            }

            _context.Projects.Add(ProjectModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
