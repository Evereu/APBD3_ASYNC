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

        public void AddNewProduct(Warehouse warehouse)  
        {

            if (_repository.VerifyExistingProduct(warehouse) && _repository.VerifyExistingWarehouse(warehouse))
            {
                if(_repository.VerifyExistingOrder(warehouse) == false && _repository.VerifyCompletedOrders(warehouse))
                {
                    _repository.InsertNewOrder(warehouse);
                }
                else
                {
                    //print - order istnieje
                }
            }
            else
            {
                // print - tu ma zwracać że nie istnieje dany magazyn lub produkt
            }
        }
    }
}
