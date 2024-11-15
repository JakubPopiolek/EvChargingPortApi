using EvApplicationApi.DTOs;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

        [HttpGet("{applicationReference}")]
        public ActionResult<ApplicationItemDto> GetApplicationItem(Guid applicationReference)
        {
            var applicationItem = _applicationRepository
                .GetApplicationItemPublic(applicationReference)
                .Result;

            if (applicationItem == null)
            {
                return NotFound();
            }

            return Ok(applicationItem);
        }

        [HttpPut]
        [Route("submitApplication")]
        public async Task<ActionResult<ApplicationItem>> SubmitApplication(
            ApplicationItem applicationItem
        )
        {
            if (applicationItem.ReferenceNumber == Guid.Empty)
            {
                return BadRequest("Missing Guid");
            }

            ApplicationItem? applicationInDb = await _applicationRepository.GetApplicationItem(
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
        [Route("startApplication")]
        [EnableRateLimiting("startApplication_fixed")]
        public ActionResult<Guid> StartApplication()
        {
            Guid applicationGuid = _applicationRepository.StartApplication();
            _applicationRepository.Save();
            return applicationGuid;
        }
    }
}
