using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationItemsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ApplicationItemsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationItem>>> GetApplicationItems()
        {
            return await _context.ApplicationItems.ToListAsync();
        }

        // GET: api/ApplicationItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationItem>> GetApplicationItem(long id)
        {
            var applicationItem = await _context.ApplicationItems.FindAsync(id);

            if (applicationItem == null)
            {
                return NotFound();
            }

            return applicationItem;
        }

        // PUT: api/ApplicationItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationItem(
            long id,
            ApplicationItem applicationItem
        )
        {
            if (id != applicationItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationItemExists(id))
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

        // POST: api/ApplicationItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationItem>> PostApplicationItem(
            ApplicationItem applicationItem
        )
        {
            _context.ApplicationItems.Add(applicationItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetApplicationItem",
                new { id = applicationItem.Id },
                applicationItem
            );
        }

        // DELETE: api/ApplicationItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationItem(long id)
        {
            var applicationItem = await _context.ApplicationItems.FindAsync(id);
            if (applicationItem == null)
            {
                return NotFound();
            }

            _context.ApplicationItems.Remove(applicationItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationItemExists(long id)
        {
            return _context.ApplicationItems.Any(e => e.Id == id);
        }
    }
}
