using LeaveManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagement.Utils
{
    public class StaticDropDowns
    {
        public static List<SelectListItem> GetLeaveType()
        {
            List<SelectListItem> valueList =
            [
                new SelectListItem() { Text = nameof(LeaveType.Annual), Value = nameof(LeaveType.Annual) },
                new SelectListItem() { Text = nameof(LeaveType.Sick), Value = nameof(LeaveType.Sick) },
                new SelectListItem() { Text = nameof(LeaveType.Unpaid), Value = nameof(LeaveType.Unpaid) },
                new SelectListItem() { Text = nameof(LeaveType.Maternity), Value = nameof(LeaveType.Maternity) }
            ];

            return valueList;
        }

        public static List<SelectListItem> GetLeaveStatus()
        {
            List<SelectListItem> valueList =
            [
                new SelectListItem() { Text = nameof(LeaveStatus.Pending), Value = nameof(LeaveStatus.Pending) },
                new SelectListItem() { Text = nameof(LeaveStatus.Approved), Value = nameof(LeaveStatus.Approved) },
                new SelectListItem() { Text = nameof(LeaveStatus.Rejected), Value = nameof(LeaveStatus.Rejected) },
            ];

            return valueList;
        }
    }
}
