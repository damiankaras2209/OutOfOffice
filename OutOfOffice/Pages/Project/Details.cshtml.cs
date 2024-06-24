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

namespace OutOfOffice.Pages.Project
{
    public class DetailsModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;

        public DetailsModel (
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

            var projectmodel = await _context.Projects.Include(p => p.ProjectManager).FirstOrDefaultAsync(m => m.ID == id);
            if (projectmodel == null)
            {
                return NotFound();
            }
            else
            {
                ProjectModel = projectmodel;
            }
            return Page();
        }
    }
}
