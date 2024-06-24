using OutOfOffice.Data;
using OutOfOffice.Models;

namespace OutOfOffice.Repositories
{
    public class LeaveRequestManager
    {
        private ApplicationDbContext _context;

        public LeaveRequestManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LeaveRequestModel> GetLeaveRequests()
        {
            var list = _context.LeaveRequests
                //.Include(a => a.Vendor)
                //.Include(a => a.Transmission)
                .ToList();

            //list.ForEach(a => a.isRented = _context.Rents.Where(b => b.VehicleId == a.Id).Where(b => b.Status == RentModel.RentStatus.ACTIVE).Any());
            return list;
        }
    }
}
