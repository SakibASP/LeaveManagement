using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagement.Models
{
    [Table(nameof(LeaveRequest))]
    public class LeaveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string LeaveType { get; set; } 
        [Required]
        public string Status { get; set; } = LeaveStatus.Pending.ToString();
        public string Reason { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
    }

    public enum LeaveStatus
    {
        Pending, Approved, Rejected

    }

    public enum LeaveType
    {
        Annual, Sick, Unpaid, Maternity
    }
}
