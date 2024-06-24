using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Pages.Project
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
        public string ProjectTypeSort { get; set; }
        public string ProjectManagerSort { get; set; }
        public string StartDateSort { get; set; }
        public string EndDateSort { get; set; }
        public string CommentSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentSort { get; set; }


        public string IdSearch { get; set; }
        public string ProjectManagerFilter { get; set; }


        public IList<ProjectModel> Project { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string sortOrder, string idSearch, string managerSearch)
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

            CurrentSort = sortOrder;
            IDSort = sortOrder == "id" ? "id_desc" : "id";
            ProjectTypeSort = sortOrder == "type" ? "type_desc" : "type";
            ProjectManagerSort = sortOrder == "manager" ? "manager_desc" : "manager";
            StartDateSort = sortOrder == "start" ? "start_desc" : "start";
            EndDateSort = sortOrder == "end" ? "end_desc" : "end";
            CommentSort = sortOrder == "comment" ? "comment_desc" : "comment";
            StatusSort = sortOrder == "status" ? "status_desc" : "status";

            IdSearch = idSearch;
            ProjectManagerFilter = managerSearch;

            IQueryable<ProjectModel> projectsIQ = from p in _context.Projects.Include(p => p.ProjectManager)
                                                  select p;

            if (! _access.HasAccess(CurrentUser, AccessResourceModel.AccessResource.ProjectsFullList))
            {
                System.Diagnostics.Debug.WriteLine(CurrentUser.Id);
                System.Diagnostics.Debug.WriteLine("1");
                var user = _context.Employees.Include(e => e.Project).Where(e => e.Id == CurrentUser.Id).FirstOrDefault();
                if (user.Project != null)
                {
                    projectsIQ = projectsIQ.Where(p => p.ID == user.Project.ID);
                }
                else
                {
                    projectsIQ = projectsIQ.Where(p => p.ID == -1);
                }
            }


            if (!String.IsNullOrEmpty(idSearch))
                projectsIQ = projectsIQ.Where(p => p.ID.ToString() == idSearch);
            if (!String.IsNullOrEmpty(managerSearch))
                projectsIQ = projectsIQ.Where(p => p.ProjectManager.FullName.Contains(managerSearch));

            switch (sortOrder)
            {
                case "id": projectsIQ = projectsIQ.OrderBy(p => p.ID); break;
                case "id_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.ID); break;
                case "type": projectsIQ = projectsIQ.OrderBy(p => p.ProjectType); break;
                case "type_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.ProjectType); break;
                case "manager": projectsIQ = projectsIQ.OrderBy(p => p.ProjectManager.FullName); break;
                case "manager_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.ProjectManager.FullName); break;
                case "start": projectsIQ = projectsIQ.OrderBy(p => p.StartDate); break;
                case "start_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.StartDate); break;
                case "end": projectsIQ = projectsIQ.OrderBy(p => p.EndDate); break;
                case "end_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.EndDate); break;
                case "comment": projectsIQ = projectsIQ.OrderBy(p => p.Comment); break;
                case "comment_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.Comment); break;
                case "status": projectsIQ = projectsIQ.OrderBy(p => p.Status); break;
                case "status_desc": projectsIQ = projectsIQ.OrderByDescending(p => p.Status); break;
                default:
                    projectsIQ = projectsIQ.OrderBy(p => p.ID);
                    break;
            }

            Project = await projectsIQ.AsNoTracking().ToListAsync();
            return Page();
        }
    }
}
