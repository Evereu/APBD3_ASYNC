using APBD3_ASYNC.Models;

namespace APBD3_ASYNC.Services
{
    public interface IWarehouseService
    {
        Task<string> AddNewProductQuery(Warehouse warehouse);
    }
}