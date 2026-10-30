using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using TechStore.Api.Models;
using TechStore.Api.Services;

namespace TechStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechStoreProductController : ControllerBase
    {
        public readonly ITechStoreServices _service;

        public TechStoreProductController(ITechStoreServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Produs>>> GetAllProduses()
        {
            return Ok(await _service.GetAllProduses());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produs>> GetProdusById(int id)
        {
            var produs = await _service.GetProdusById(id);
            if (produs == null)
                return NotFound("Produsul cu id-ul dat nu a fost gasit");

            return Ok(produs);
        }

        [HttpPost]
        public async Task<ActionResult<Produs>> AddProdus(Produs produs)
        {
            return Ok(await _service.AddProdus(produs));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProdus(int id)
        {
            var succes = await _service.DeleteProdus(id);
            if (!succes) return NotFound("Produsul nu a fost găsit pentru ștergere.");

            return Ok(succes);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProdus(int id, Produs produs)
        {
            var succes = await _service.UpdateProdus(id, produs);
            if (!succes) return NotFound("Produsul nu a fost găsit pentru actualizare.");

            return Ok(succes);
        }
    }
}