using APBD3_ASYNC.Models;

namespace APBD3_ASYNC.Repository
{
    public interface IWarehouseRepository
    {
        Task<bool> IsThatCompletedOrdersExist(Warehouse warehouse);
        Task<bool> VerifyExistingOrder(Warehouse warehouse);
        Task<bool> VerifyExistingProduct(Warehouse warehouse);
        Task<bool> VerifyExistingWarehouse(Warehouse warehouse);
        Task<decimal> InsertNewOrder(Warehouse warehouse);

    }
}