using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Data;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.Project
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

        [BindProperty]
        public ProjectModel ProjectModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var projectmodel =  await _context.Projects.FirstOrDefaultAsync(m => m.ID == id);
            if (projectmodel == null)
            {
                return NotFound();
            }
            ProjectModel = projectmodel;
            PopulateViewData();
            return Page();
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
            if (!_access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.ProjectsList))
            {
                return Forbid();
            }

            _context.Attach(ProjectModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectModelExists(ProjectModel.ID))
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

        private bool ProjectModelExists(int id)
        {
            return _context.Projects.Any(e => e.ID == id);
        }
    }
}
