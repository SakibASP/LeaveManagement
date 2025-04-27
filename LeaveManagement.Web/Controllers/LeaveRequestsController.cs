using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeaveManagement.Infrustructure.Data;
using LeaveManagement.Models;
using Microsoft.AspNetCore.Authorization;
using LeaveManagement.Utils;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using LeaveManagement.Infrustructure.Services;
using System.Security.Claims;
using LeaveManagement.Web.Helper;

namespace LeaveManagement.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly EmailService _emailService;

        public LeaveRequestsController(ApplicationDbContext context, IHttpClientFactory httpClientFactory,EmailService emailService)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7265/api/"); // Adjust the base address as needed
            _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
            _emailService = emailService;
        }
        // GET: LeaveRequests
        public async Task<IActionResult> Index()
        {
            string jwtToken = await JWTTokenHelper.GetToken(_httpClient, "admin", "123");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var response = await _httpClient.GetAsync("LeaveRequests");
            var responseString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IList<LeaveRequest>>(responseString);
            return View(model);
        }

        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"LeaveRequests/{id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var leaveRequest = JsonConvert.DeserializeObject<LeaveRequest>(responseString);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name");
            ViewData["LeaveType"] = new SelectList(StaticDropDowns.GetLeaveType(), "Value", "Text");
            ViewData["Status"] = new SelectList(StaticDropDowns.GetLeaveStatus(), "Value", "Text");
            return View();
        }

        // POST: LeaveRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,StartDate,EndDate,LeaveType,Status,Reason,AppliedDate")] LeaveRequest leaveRequest)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(leaveRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("LeaveRequests", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Added!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Failed!";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch { }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", leaveRequest.EmployeeId);
            ViewData["LeaveType"] = new SelectList(StaticDropDowns.GetLeaveType(), "Value", "Text", leaveRequest.LeaveType);
            ViewData["Status"] = new SelectList(StaticDropDowns.GetLeaveStatus(), "Value", "Text", leaveRequest.Status);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"LeaveRequests/{id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var leaveRequest = JsonConvert.DeserializeObject<LeaveRequest>(responseString);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", leaveRequest.EmployeeId);
            ViewData["LeaveType"] = new SelectList(StaticDropDowns.GetLeaveType(), "Value", "Text", leaveRequest.LeaveType);
            ViewData["Status"] = new SelectList(StaticDropDowns.GetLeaveStatus(), "Value", "Text", leaveRequest.Status);
            return View(leaveRequest);
        }

        // POST: LeaveRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,StartDate,EndDate,LeaveType,Status,Reason,AppliedDate")] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.Id)
            {
                return NotFound();
            }

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(leaveRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"LeaveRequests/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Updated!";
                    try
                    {
                        string? emp = _context.Employee.Find(leaveRequest.EmployeeId)?.Name;
                        const string subject = "Leave Request Update!";
                        string htmlMessage = $@"<h5>Hello {User.Identity?.Name},</h5>
                            <p>Request updated of employe {emp}.</p>
                            <p>Current status <strong>{leaveRequest.Status}</strong>.</p>";
                        string? sendTo = User.FindFirst(ClaimTypes.Email)?.Value;
                        if(!string.IsNullOrEmpty(sendTo))
                        {
                            await _emailService.SendEmailAsync(sendTo, subject, htmlMessage);
                        }
                    }
                    catch { }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Failed!";
                    return RedirectToAction(nameof(Index));

                }

            }
            catch
            {

            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Name", leaveRequest.EmployeeId);
            ViewData["LeaveType"] = new SelectList(StaticDropDowns.GetLeaveType(), "Value", "Text", leaveRequest.LeaveType);
            ViewData["Status"] = new SelectList(StaticDropDowns.GetLeaveStatus(), "Value", "Text", leaveRequest.Status);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"LeaveRequests/{id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var leaveRequest = JsonConvert.DeserializeObject<LeaveRequest>(responseString);

            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"LeaveRequests/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Removed!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = "Failed!";
                return RedirectToAction(nameof(Index));

            }
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequest.Any(e => e.Id == id);
        }
    }
}
