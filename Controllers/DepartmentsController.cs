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
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(departments.Select(d => new DepartmentDTO
            {
                Id = d.Id,
                Name = d.Name,
            }));
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,
            });
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(Guid id, DepartmentDTO departmentDTO)
        {
            if (id != departmentDTO.Id)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.Name = departmentDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDTO>> PostDepartment(DepartmentDTO departmentDTO)
        {
            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = departmentDTO.Name
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.Id }, new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name
            });
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(Guid id)
        {
            return _context.Departments.Any(d => d.Id == id);
        }
    }
}
