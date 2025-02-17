
USE LeaveManagement;

/**

Each Leave Request should have:
• Id (int, primary key)
• EmployeeId (int, foreign key)
• StartDate (DateTime, required)
• EndDate (DateTime, required)
• LeaveType (enum: Annual, Sick, Unpaid, Maternity)
• Status (enum: Pending, Approved, Rejected)
• Reason (string, optional)
• AppliedDate (DateTime, auto-generated)

**/

CREATE TABLE Employee(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
Name NVARCHAR(256) NOT NULL
)

GO

CREATE TABLE LeaveRequest(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED,
EmployeeId INT REFERENCES Employee(Id),
StartDate DateTime NOT NULL,
EndDate DateTime NOT NULL,
LeaveType VARCHAR(128) NOT NULL,
Status VARCHAR(128) NOT NULL,
Reason NVARCHAR(MAX) NULL,
AppliedDate DateTime DEFAULT(GETDATE())
)