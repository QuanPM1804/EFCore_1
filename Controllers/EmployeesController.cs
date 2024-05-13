using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCore_1.Data;
using EFCore_1.Models;
using EFCore_1.DTOs;

namespace EFCore_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentId = e.DepartmentId,
                JoinedDate = e.JoinedDate,
            }));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                DepartmentId = employee.DepartmentId,
                JoinedDate = employee.JoinedDate,
            });
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                return BadRequest();
            }

            var employee = await _context.Employees.FindAsync();

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = employeeDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(EmployeeDTO employeeDTO)
        {
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = employeeDTO.Name,
                DepartmentId = employeeDTO.DepartmentId,
                JoinedDate = employeeDTO.JoinedDate,
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, new EmployeeDTO
            {
                Id =employee.Id,
                Name = employeeDTO.Name,
                DepartmentId = employeeDTO.DepartmentId,
                JoinedDate=employeeDTO.JoinedDate,
            });
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("EmployeesWithDepartments")]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployeesWithDepartments()
        {
            var employees = await _context.Employees
                .Join(_context.Departments, e => e.DepartmentId, d => d.Id, (e, d) => new { Employee = e, Department = d })
                .Select(x => new { x.Employee.Name, DepartmentName = x.Department.Name })
                .ToListAsync();

            return Ok(employees);
        }

        [HttpGet("EmployeesWithProjects")]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployeesWithProjects()
        {
            var employees = await _context.Employees.SelectMany(e => e.ProjectEmployees.DefaultIfEmpty(),(e, pe) => new
            {
                EmployeeName = e.Name,
                ProjectName = pe == null || pe.Project == null ? "No Project" : pe.Project.Name
            }).ToListAsync();

            return Ok(employees);
        }

        [HttpGet("EmployeesWithSalaryAndJoinedDate")]
        public async Task<ActionResult<IEnumerable<object>>> GetEmployeesWithSalaryAndJoinedDate()
        {
            var employees = await _context.Employees
                .Where(e => e.Salary.SalaryAmount > 100 && e.JoinedDate >= new DateTime(2024, 1, 1))
                .Select(e => new { e.Name, e.Salary.SalaryAmount, e.JoinedDate })
                .ToListAsync();

            return Ok(employees);
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
