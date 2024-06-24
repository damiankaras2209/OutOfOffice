using OutOfOffice.Helpers;

namespace OutOfOffice.Models
{
    public class AccessResourceModel
    {
        public enum AccessResource
        {
            EmployeesList = 0,
            EmployeesAssignProject = 1,
            LeaveRequestsList = 2,
            LeaveRequestsFullList = 3,
            LeaveRequestsCreateAndManage = 4,
            ApprovalRequestsList = 5,
            ProjectsList = 6,
            ProjectsManage = 7,
            ProjectsFullList = 8,
        }

        public AccessResource Resource { get; set; }
        public Position Position { get; set; }

        public bool HasAccess { get; set; }

    }
}
