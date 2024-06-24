using OutOfOffice.Data;
using OutOfOffice.Models;

namespace OutOfOffice.Repositories
{
    public class EmployeeManager
    {
        private ApplicationDbContext _context;

        public EmployeeManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<EmployeeModel> GetEmployees()
        {
            var list = _context.Employees
                //.Include(a => a.Vendor)
                //.Include(a => a.Transmission)
                .ToList();

            //list.ForEach(a => a.isRented = _context.Rents.Where(b => b.VehicleId == a.Id).Where(b => b.Status == RentModel.RentStatus.ACTIVE).Any());
            return list;
        }
    }
}
