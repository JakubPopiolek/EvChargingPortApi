using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public ActionResult<ApplicationItem> GetApplicationItem(Guid id)
        {
            var applicationItem = _applicationRepository.GetApplicationItem(id);

            if (applicationItem == null)
            {
                return NotFound();
            }

            return Ok(applicationItem);
        }

        [HttpPut]
        public ActionResult<ApplicationItem> SubmitApplication(ApplicationItem applicationItem)
        {
            if (applicationItem.ReferenceNumber == Guid.Empty)
            {
                return BadRequest("Missing Guid");
            }

            ApplicationItem applicationInDb = _applicationRepository.GetApplicationItem(
                applicationItem.ReferenceNumber
            );
            if (applicationInDb != null)
            {
                applicationInDb.Address = applicationItem.Address;
                applicationInDb.FirstName = applicationItem.FirstName;
                applicationInDb.LastName = applicationItem.LastName;
                applicationInDb.Vrn = applicationItem.Vrn;
                applicationInDb.Email = applicationItem.Email;

                _applicationRepository.SubmitApplication(applicationInDb);
                _applicationRepository.Save();

                return Ok(applicationItem);
            }
            else
            {
                return NotFound(
                    "Could not find application with GUID: " + applicationItem.ReferenceNumber
                );
            }
        }

        [HttpPost]
        public ActionResult<Guid> BeginApplication()
        {
            Guid applicationGuid = _applicationRepository.BeginApplication();
            _applicationRepository.Save();
            return applicationGuid;
        }

        // GET: api/ApplicationItems
        // [NonAction]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ApplicationItem>>> GetApplicationItems()
        // {
        //     return await _context.ApplicationItems.ToListAsync();
        // }

        // // GET: api/ApplicationItems/5

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
