using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Models;
using LeaveManagement.Infrustructure.Data;

namespace LeaveManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        // GET: api/LeaveRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests()
        {
            return Ok(await _context.LeaveRequest.ToListAsync());
        }

        // GET: api/LeaveRequests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequest.FindAsync(id);
            return leaveRequest is not null ? Ok(leaveRequest) : NotFound();
        }

        // POST: api/LeaveRequests
        [HttpPost]
        public async Task<ActionResult<LeaveRequest>> PostLeaveRequest([FromBody] LeaveRequest leaveRequest)
        {
            if (leaveRequest == null) return BadRequest("Invalid request data.");

            try
            {
                await _context.LeaveRequest.AddAsync(leaveRequest);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetLeaveRequest), new { id = leaveRequest.Id }, leaveRequest);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // PUT: api/LeaveRequests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveRequest(int id, [FromBody] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.Id) return BadRequest("Mismatched request ID.");

            _context.Entry(leaveRequest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest($"Concurrency Error: {ex.Message}");
            }
        }

        // DELETE: api/LeaveRequests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequest.FindAsync(id);
            if (leaveRequest is null) return NotFound();

            try
            {
                _context.LeaveRequest.Remove(leaveRequest);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
