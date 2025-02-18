# LeaveManagement

1. Run the 'CreateDatabase.sql' script from 'Db Scripts' folder in SSMS.
2. Change the connection string in appSettings.json (for both LeaveManagement.Web and LeaveManagement.WebApi).
3. From solution properties select multiple startup project, then select LeaveMananagement.Web and LeaveManagement.WebApi and Debug Target as 'https' for both.
4. Cross check if the Properties/launchSettings.json of LeaveManagement.WebApi has the same url and port number defined in the httpClient BaseAddress of LeaveRequestController's constructor in LeaveManagement.Web.
5. Cross check if the Properties/launchSettings.json of LeaveManagement.Web has the same urls and port number defined in the allowedOrigins in program.cs in LeaveManagement.WebApi.
6. Build and run the project.
