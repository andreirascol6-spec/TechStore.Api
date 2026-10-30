using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Api.Models;
using TechStore.Api.Services;

namespace TechStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechStoreOrdersController : ControllerBase
    {
        private readonly ITechStoreOrdersServices _service;

        public TechStoreOrdersController(ITechStoreOrdersServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comanda>>> GetAllOrders()
        {
            return Ok(await _service.GetAllOrders());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comanda>> GetOrderById(int id)
        {
            var comanda = await _service.GetOrderById(id);
            if (comanda == null)
            {
                return NotFound("Comanda cu Id-ul introdus nu a fost găsită.");
            }
            return Ok(comanda);
        }

        [HttpPost]
        public async Task<ActionResult<Comanda>> AddOrder(Comanda comanda_noua)
        {
            var rezult = await _service.AddOrder(comanda_noua);
            if (rezult == null)
            {
                return BadRequest("Comanda nu a putut fi plasată. Stoc insuficient sau produs inexistent.");
            }
            return Ok(rezult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteOrder(int id)
        {
            var succes = await _service.DeleteOrder(id);
            if (!succes)
            {
                return NotFound("Comanda cu Id-ul dat nu a fost găsită pentru ștergere.");
            }
            return Ok(succes);
        }
    }
}    