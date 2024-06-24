using OutOfOffice.Data;
using OutOfOffice.Models;

namespace OutOfOffice.Repositories
{
    public class ProjectManager
    {
        private ApplicationDbContext _context;

        public ProjectManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ProjectModel> GetApprovalRequestModels()
        {
            var list = _context.Projects
                //.Include(a => a.Vendor)
                //.Include(a => a.Transmission)
                .ToList();

            //list.ForEach(a => a.isRented = _context.Rents.Where(b => b.VehicleId == a.Id).Where(b => b.Status == RentModel.RentStatus.ACTIVE).Any());
            return list;
        }
    }
}
