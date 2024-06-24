using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutOfOffice.Helpers;

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
