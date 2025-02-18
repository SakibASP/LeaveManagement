# LeaveManagement

1. Run the 'CreateDatabase.sql' Script from 'Db Scripts' folder. 
2. Change the connection string in appSettings.json ( for both LeaveManagement.Web and LeaveManagement.WebApi )
3. From solution properties select multiple startup project
4. Then select LeaveMananagement.Web and LeaveManagement.WebApi and Debug target 'https' for both
5. Cross check if the launchSettings.json of LeaveManagement.Web and LeaveManagement.WebApi have the same url and port number defined as like allowedOrigins in program.cs of LeaveManagement.WebApi  and httpClient BaseAddress of LeaveRequestController's constructor in LeaveManagement.Web
6. Run the project
