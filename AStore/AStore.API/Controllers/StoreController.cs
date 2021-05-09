using AStore.Library.Repository;
using AStore.Library.Extensions;
using AStore.Library.Models;
using AStore.Library.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository repository;
        public StoreController(IStoreRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost(@"Enter/{section}/{timestamp}")]
        public async Task<ActionResult> EnterAsync(string section, DateTimeOffset timestamp)
        {
            Customer customerEnter = new()
            {
                SectionName = section,
                Timestamp = timestamp
            };

            await repository.AddCustomerAsync(customerEnter);

            return Ok();
        }

        [HttpPost("Exit/{section}/{timestamp}")]
        public async Task<ActionResult> ExitAsync(string section, DateTimeOffset timestamp)
        {
            Customer customerExit = new()
            {
                SectionName = section,
                Timestamp = timestamp
            };

            await repository.RemoveCustomerAsync(customerExit);

            return Ok();
        }

        [HttpGet("ListSections")]
        public async Task<IEnumerable<SectionDto>> GetSectionsAsync()
        {
            var sectionList = (await repository.GetSectionListAsync())
                              .Select(section => section.AsDto());

            return sectionList;
        }

        [HttpDelete("ResetSection")]
        public async Task<IActionResult> ResetSectionAsync(string section)
        {
            var existingCustomer = await repository.GetSectionAsync(section);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            await repository.ResetSectionAsync(section);

            return NoContent();
        }
    }
}
