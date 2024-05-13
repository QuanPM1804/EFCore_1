using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCore_1.Data;
using EFCore_1.DTOs;
using EFCore_1.Models;

namespace EFCore_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryDTO>>> GetSalaries()
        {
            var salaries = await _context.Salaries.ToListAsync();
            return Ok(salaries.Select(s => new SalaryDTO
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                SalaryAmount = s.SalaryAmount,
            }));
        }
        // GET: api/Salaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryDTO>> GetSalary(Guid id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            return Ok(new SalaryDTO
            {
                Id = salary.Id,
                EmployeeId = salary.EmployeeId,
                SalaryAmount = salary.SalaryAmount,
            });
        }
        // PUT: api/Salaries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalary(Guid id, SalaryDTO salaryDTO)
        {
            if (id != salaryDTO.Id)
            {
                return BadRequest();
            }

            var salary = await _context.Salaries.FindAsync(id);

            if (salary == null)
            {
                return NotFound();
            }
            salary.SalaryAmount = salaryDTO.SalaryAmount;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Salaries
        [HttpPost]
        public async Task<ActionResult<SalaryDTO>> PostSalary(SalaryDTO salaryDTO)
        {
            var salary = new Salary
            {
                Id = Guid.NewGuid(),
                EmployeeId = salaryDTO.EmployeeId,
                SalaryAmount = salaryDTO.SalaryAmount
            };

            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalary), new { id = salary.Id }, new SalaryDTO
            {
                Id = salaryDTO.Id,
                EmployeeId = salaryDTO.EmployeeId,
                SalaryAmount = salary.SalaryAmount
            });
        }

        // DELETE: api/Salaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalary(Guid id)
        {
            var salary = await _context.Salaries.FindAsync(id);

            if (salary == null)
            {
                return NotFound();
            }

            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalaryExists(Guid id)
        {
            return _context.Salaries.Any(s => s.Id == id);
        }
    }
}
