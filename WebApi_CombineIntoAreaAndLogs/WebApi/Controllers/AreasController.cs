using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HandleException]
    public class AreasController : ControllerBase
    {
        private readonly CoalWarehouseService _service;

        public AreasController(CoalWarehouseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(AreaInputView input)
        {
            var areaInput = new AreaInput(input.WarehouseId.Value, input.PicketIds);
            await _service.CombinePicketsToArea(areaInput);
            return Ok();
        }
    }
}
