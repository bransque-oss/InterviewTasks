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
    public class PicketsController : ControllerBase
    {
        private readonly CoalWarehouseService _service;

        public PicketsController(CoalWarehouseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picket>>> GetAll(int warehouseId)
        {
            if (warehouseId <= 0) 
            {
                return BadRequest("WarehouseId should be greater than zero.");
            }
            return Ok(await _service.GetPickets(warehouseId));
        }

        [HttpGet("history/{warehouseId:min(1)}")]
        public async Task<ActionResult<PicketAreaView>> GetHistory(int warehouseId, DateTime start, DateTime end)
        {
            var history = await _service.GetPicketAreaHistory(warehouseId, start, end);
            var picketAreaView = new PicketAreaView
            {
                WarehouseName = history.First().WarehouseName
            };
            var areas = new List<AreaView>();
            foreach (var areaGroup in history.GroupBy(x => new { x.AreaName, x.Created }))
            {
                var area = new AreaView
                {
                    AreaName = areaGroup.Key.AreaName,
                    PicketNumbers = areaGroup.Select(x => x.PicketNumber),
                    Weight = areaGroup.Select(x => x.Weight).Sum(),
                    Created = areaGroup.Key.Created
                };
                areas.Add(area);
            }
            picketAreaView.Areas = areas;
            return Ok(picketAreaView);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(PicketInputView input)
        {
            var picket = new PicketInput(input.Number.Value, input.WarehouseId.Value);
            var picketId = await _service.AddPicket(picket);
            return Ok($"Picket Id = '{picketId}'.");
        }

        [HttpPut("{id:min(1)}")]
        public async Task<ActionResult<int>> Update(int id, PicketInputView input)
        {
            var picket = new PicketInput(input.Number.Value, input.WarehouseId.Value);
            await _service.UpdatePicket(id, picket);
            return Ok();
        }

        [HttpDelete("{id:min(1)}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await _service.DeletePicket(id);
            return Ok();
        }
    }
}
