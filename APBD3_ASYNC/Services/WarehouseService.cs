using APBD3_ASYNC.Models;
using APBD3_ASYNC.Repository;

namespace APBD3_ASYNC.Services
{
    public class WarehouseService : IWarehouseService
    {

        private readonly IWarehouseRepository _repository;

        public WarehouseService(IWarehouseRepository warehouseRepository) 
        { 

            _repository = warehouseRepository;

        }
            
        public string AddNewProductQuery(Warehouse warehouse)  
        {

            if (_repository.VerifyExistingProduct(warehouse) && _repository.VerifyExistingWarehouse(warehouse))
            {
                if(_repository.VerifyExistingOrder(warehouse) == false && _repository.VerifyCompletedOrders(warehouse))
                {
                   return _repository.InsertNewOrder(warehouse).ToString();
                }
                else
                {
                    return "Taki order już istnieje";
                }
            }
            else
            {
                return "magazyn lub produkt nie istnieje";
            }
        }
    }
}
