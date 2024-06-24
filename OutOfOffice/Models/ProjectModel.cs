using OutOfOffice.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.Models
{
    public class ProjectModel
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Project Type")]
        public ProjectType ProjectType { get; set; }

        [Required]
        [DisplayName("Start Date")]
        public DateOnly StartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        public DateOnly EndDate { get; set; }


        //[Required]
        [DisplayName("Project Manager")]
        public virtual EmployeeModel ProjectManager { get; set; }
        [ForeignKey("ProjectManager")]
        public String ProjectManagerId { get; set; }

        public String Comment { get; set; }

        [Required]
        public Status Status { get; set; }

    }
}
