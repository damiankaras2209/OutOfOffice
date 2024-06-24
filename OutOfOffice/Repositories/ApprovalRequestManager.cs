using OutOfOffice.Data;
using OutOfOffice.Models;

namespace OutOfOffice.Repositories
{
    public class ApprovalRequestManager
    {
        private ApplicationDbContext _context;

        public ApprovalRequestManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ApprovalRequestModel> GetApprovalRequestModels()
        {
            var list = _context.ApprovalRequests
                //.Include(a => a.Vendor)
                //.Include(a => a.Transmission)
                .ToList();

            //list.ForEach(a => a.isRented = _context.Rents.Where(b => b.VehicleId == a.Id).Where(b => b.Status == RentModel.RentStatus.ACTIVE).Any());
            return list;
        }
    }
}

