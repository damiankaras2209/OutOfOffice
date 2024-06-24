using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Linq;
using OutOfOffice.Data;
using OutOfOffice.Helpers;
using OutOfOffice.Models;
using static System.Formats.Asn1.AsnWriter;

namespace OutOfOffice.Pages.Employee
{
    public class CreateModel : PageModel
    {
        private readonly OutOfOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly Access _access;
        private readonly UserStore<EmployeeModel> _userStore;

        public CreateModel(
            OutOfOffice.Data.ApplicationDbContext context,
            UserManager<EmployeeModel> userManager,
            Access access,
            UserStore<EmployeeModel> userStore
        )
        {
            _context = context;
            _userManager = userManager;
            _access = access;
            _userStore = userStore;
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


        public IActionResult OnGet()
        {
            PopulateViewData();
            return Page();
        }

        [BindProperty]
        public EmployeeModel EmployeeModel { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
            var user = CreateUser();

            var id = _context.Employees.Max(x => x.NID) + 1;

            user.NID = id;
            user.FullName = EmployeeModel.FullName;
            user.PeoplePartnerId = EmployeeModel.PeoplePartnerId;
            user.Balance = EmployeeModel.Balance;
            user.Subdivision = EmployeeModel.Subdivision;
            user.Position = EmployeeModel.Position;
            user.Status = EmployeeModel.Status;

            await _userStore.SetUserNameAsync(user, EmployeeModel.UserNameInput, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, EmployeeModel.PasswordInput);

            if (result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                return RedirectToPage("./Index");
            }
            foreach (var error in result.Errors)
            {
                System.Diagnostics.Debug.WriteLine(error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }

            PopulateViewData();
            return Page();
        }

        private EmployeeModel CreateUser()
        {
            try
            {
                return Activator.CreateInstance<EmployeeModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(EmployeeModel)}'. " +
                    $"Ensure that '{nameof(EmployeeModel)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
