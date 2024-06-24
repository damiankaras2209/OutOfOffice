using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutOfOffice.Helpers;
using OutOfOffice.Models;

namespace OutOfOffice.Models
{

    public class LeaveRequestModel
    {

        public int ID { get; set; }

        [Required]
        public virtual EmployeeModel Employee { get; set; }
        [ForeignKey("Employee")]
        public String EmployeeId { get; set; }

        [Required]
        [DisplayName("Absence Reason")]
        public AbsenceReason AbsenceReason { get; set; }

        [Required]
        [DisplayName("Start Date")]
        public DateOnly StartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        public DateOnly EndDate { get; set; }

        public String? Comment { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.New;


    }

}