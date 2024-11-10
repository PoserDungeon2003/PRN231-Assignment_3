using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SilverPE_Repository.Interfaces;
using SilverPE_Repository.Request;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SilverPE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SilverJewelryController : ControllerBase
    {
        private readonly IJewelryRepository _jewelryRepository;

        public SilverJewelryController(IJewelryRepository jewelryRepository)
        {
            _jewelryRepository = jewelryRepository;
        }

        // GET: api/<SilverJewelryController>
        [EnableQuery]
        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllJewerly()
        {
            var response = await _jewelryRepository.GetJewelries();
            return Ok(response);
        }

        [HttpGet("search")]
        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> SearchByNameOrWeight([FromQuery] string? searchValue)
        {
            var response = await _jewelryRepository.SearchByNameOrWeight(searchValue ?? "");
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }

        // POST api/<SilverJewelryController>
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CreateSilverJewerlry([FromBody] CreateSilverJewerlryRequest body)
        {
            var response = await _jewelryRepository.AddJewelry(body);

            if (response)
            {
                return Ok(new
                {
                    Success = response,
                });
            }
            return BadRequest(new
            {
                Success = response,
                Message = "Failed to add jewelry",
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetSilverJewerlyById(string id)
        {
            var response = await _jewelryRepository.GetSilverJewelryById(id);

            if (response != null)
            {
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateSilverJewerlyById(string id, [FromBody] UpdateSilverJewerlyRequest value)
        {
            var response = await _jewelryRepository.UpdateJewelry(id, value);

            if (response)
            {
                return Ok(new
                {
                    Success = response,
                });
            }

            return BadRequest(new
            {
                Success = response,
                Message = "Failed to update jewelry",
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteSilverJewerlyById(string id)
        {
            var response = await _jewelryRepository.DeleteJewelry(id);

            if (response)
            {
                return Ok(new
                {
                    Success = response,
                });
            }
            return BadRequest(new
            {
                Success = response,
                Message = "Failed to delete jewelry",
            });
        }
    }
}
