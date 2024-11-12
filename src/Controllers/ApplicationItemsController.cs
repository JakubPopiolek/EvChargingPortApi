using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using EvApplicationApi.Repository;
using EvApplicationApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationItemsController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationItemsController(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        // GET: api/ApplicationItems
        // [NonAction]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ApplicationItem>>> GetApplicationItems()
        // {
        //     return await _context.ApplicationItems.ToListAsync();
        // }

        // // GET: api/ApplicationItems/5
        [HttpGet("{id}")]
        public ActionResult<ApplicationItem> GetApplicationItem(long id)
        {
            var applicationItem = _applicationRepository.GetApplicationItem(id);

            if (applicationItem == null)
            {
                return NotFound();
            }

            return Ok(applicationItem);
        }

        // // PUT: api/ApplicationItems/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [NonAction]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutApplicationItem(
        //     Guid id,
        //     ApplicationItem applicationItem
        // )
        // {
        //     if (id != applicationItem.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(applicationItem).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ApplicationItemExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // POST: api/ApplicationItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<ApplicationItem> PostApplicationItem(ApplicationItem applicationItem)
        {
            _applicationRepository.InsertApplication(applicationItem);
            _applicationRepository.Save();

            return CreatedAtAction(
                "GetApplicationItem",
                new { id = applicationItem.Id },
                applicationItem
            );
        }

        // DELETE: api/ApplicationItems/5
        // [NonAction]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteApplicationItem(long id)
        // {
        //     var applicationItem = await _context.ApplicationItems.FindAsync(id);
        //     if (applicationItem == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.ApplicationItems.Remove(applicationItem);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        // private bool ApplicationItemExists(Guid id)
        // {
        //     return _context.ApplicationItems.Any(e => e.Id == id);
        // }
    }
}
