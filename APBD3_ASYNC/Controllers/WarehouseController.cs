using APBD3_ASYNC.Models;
using APBD3_ASYNC.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD3_ASYNC.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }   

        [HttpPost]
        public async Task<IActionResult> AddNewProduct(Warehouse warehouse)
        {

            //walidacja czy ilość przekazana w żądaniu jest większa od 0

            if (warehouse.Amount <= 0 )
            {
                return BadRequest("Amount musi być większe od zera");
            }

             var result = await _warehouseService.AddNewProductQuery(warehouse);

            return Ok(result);
        }

    }
}
