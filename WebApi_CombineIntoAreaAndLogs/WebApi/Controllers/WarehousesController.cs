using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HandleException]
    public class WarehousesController : ControllerBase
    {
        private readonly CoalWarehouseService _service;

        public WarehousesController(CoalWarehouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetAll()
        {
            return Ok(await _service.GetWarehouses());
        }
    }
}
