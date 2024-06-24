using OutOfOffice.Helpers;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models
{

    public class ApprovalRequestModel
    {

        public int ID { get; set; }

        [Required]
        public virtual EmployeeModel Approver { get; set; }

        [Required]
        public virtual LeaveRequestModel LeaveRequest { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.New;

        public String? Comment { get; set; }


    }

}
