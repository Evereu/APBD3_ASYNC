using APBD3_ASYNC.Models;

namespace APBD3_ASYNC.Repository
{
    public interface IWarehouseRepository
    {
        bool VerifyCompletedOrders(Warehouse warehouse);
        bool VerifyExistingOrder(Warehouse warehouse);
        bool VerifyExistingProduct(Warehouse warehouse);
        bool VerifyExistingWarehouse(Warehouse warehouse);

        decimal InsertNewOrder(Warehouse warehouse);
        void UpdateFullfilledAt();
    }
}