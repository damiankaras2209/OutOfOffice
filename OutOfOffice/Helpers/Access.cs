using OutOfOffice.Data;
using OutOfOffice.Models;

namespace OutOfOffice.Helpers
{
    public class Access
    {
        private readonly ApplicationDbContext _context;
        public Access(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool HasAccess(EmployeeModel? user, AccessResourceModel.AccessResource resource)
        {
            if (user == null)
            {
                return false;
            }
            return _context.AccessResources
                .Where(a => a.Position == user.Position && a.Resource == resource)
                .FirstOrDefault()
                .HasAccess;
        }
    }
}
