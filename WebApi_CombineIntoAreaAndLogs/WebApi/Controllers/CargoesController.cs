using System.Net.Sockets;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HandleException]
    public class CargoesController : ControllerBase
    {
        private readonly CoalWarehouseService _service;

        public CargoesController(CoalWarehouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetAll(int warehouseId)
        {
            if (warehouseId <=0 )
            {
                return BadRequest("WarehouseId should be greater than zero.");
            }
            return Ok(await _service.GetCargoes(warehouseId));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CargoInputView input)
        {
            var cargo = new CargoInput(input.Weight.Value, input.PicketId.Value);
            var cargoId = await _service.AddCargo(cargo);
            return Ok($"Cargo Id = '{cargoId}'.");
        }

        [HttpPut("{id:min(1)}")]
        public async Task<ActionResult<int>> Update(int id, CargoInputView input)
        {
            if (id <= 0)
            {
                return BadRequest("CargoId should be greater than zero.");
            }
            var cargo = new CargoInput(input.Weight.Value, input.PicketId.Value);
            await _service.UpdateCargo(id, cargo);
            return Ok();
        }

        [HttpDelete("{id:min(1)}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("CargoId should be greater than zero.");
            }
            await _service.DeleteCargo(id);
            return Ok();
        }
    }
}
