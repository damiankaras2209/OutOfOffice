using Microsoft.AspNetCore.Identity;
using OutOfOffice.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutOfOffice.Models
{

    public class EmployeeModel : IdentityUser
    {

        [Required]
        public int NID { get; set; }
        //public virtual string UserName { get; set; }
        //public virtual string PasswordHash { get; set; }

        [Required]
        [DisplayName("Full name")]
        public String FullName { get; set; }

        [Required]
        public Subdivision Subdivision { get; set; }

        [Required]
        public Position Position { get; set; }

        public Status Status { get; set; }

        [Required]
        [DisplayName("People Partner")]
        public virtual EmployeeModel PeoplePartner { get; set; }
        [ForeignKey("PeoplePartner")]
        public String PeoplePartnerId { get; set; }

        [Required]
        [DisplayName("Out-of-Office Balance")]
        public int Balance { get; set; }


        [DisplayName("Project")]
        public virtual ProjectModel? Project { get; set; } = null;
        [ForeignKey("Project")]
        public int? ProjectId { get; set; } = null;


        [NotMapped]
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DisplayName("Username")]
        public String UserNameInput { get; set; }

        [NotMapped]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 7)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public String PasswordInput { get; set; }

    }

}
