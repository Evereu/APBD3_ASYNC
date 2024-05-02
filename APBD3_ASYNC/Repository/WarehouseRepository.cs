using APBD3_ASYNC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace APBD3_ASYNC.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {

        private IConfiguration _configuration;

        public WarehouseRepository(IConfiguration configuration)
        {

            _configuration = configuration;

        }


        //Sprawdzamy, czy to zamówienie zostało przypadkiem zrealizowane.
        //Sprawdzamy, czy nie ma wiersza z danym IdOrder w tabeli Product_Warehouse.


        public bool VerifyCompletedOrders(Warehouse warehouse)
        {
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = " IF EXISTS " +
                        "(select * from [ApbdAsync].[dbo].[Product_Warehouse] " +
                        "where IdOrder = (Select IdOrder from [ApbdAsync].[dbo].[Order] where IdProduct = @IdProduct and Amount = @Amount)) " +
                        "BEGIN " +
                        "SELECT 1 " +
                        "END " +
                        "ELSE " +
                        "BEGIN " +
                        "SELECT 2 " +
                        "END";

                    command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);
                    command.Parameters.AddWithValue("@Amount", warehouse.Amount);

                    var queryResult = (int)command.ExecuteScalar();

                    var result = queryResult == 1 ? true : false;

                    return result;
                }
            }
        }




        //Dlatego sprawdzamy, czy w
        //tabeli Order istnieje rekord z IdProduktu i Ilością(Amount), które
        //odpowiadają naszemu żądaniu.Data utworzenia zamówienia powinna
        //być wcześniejsza niż data utworzenia w żądaniu.

        public bool VerifyExistingOrder(Warehouse warehouse)
        {
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "IF EXISTS " +
                    "(SELECT * FROM [ApbdAsync].[dbo].[Order] " +
                    "WHERE IdProduct = @IdProduct " +
                    "AND Amount >= @Amount " +
                    "AND CreatedAt < GETDATE()) " +
                    "BEGIN " +
                    "SELECT 1 " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "SELECT 2 " +
                    "END";

                    command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);
                    command.Parameters.AddWithValue("@Amount", warehouse.Amount);

                    var queryResult = (int)command.ExecuteScalar();

                    var result = queryResult == 1 ? true : false;

                    return result;
                }
            }
        }


                


        public bool VerifyExistingProduct(Warehouse warehouse)
        {
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                using (var command = new SqlCommand())
                {

                    //Sprawdz czy produkt o podanym id istnieje 

                    command.Connection = connection;

                    command.CommandText = " IF EXISTS " +
                    "(select * from [ApbdAsync].[dbo].[Product] where IdProduct = @IdProduct) " +
                    "BEGIN   " +
                    "SELECT 1 " +
                    "END " +
                    "ELSE " +
                    "BEGIN    " +
                    "SELECT 2 " +
                    "END";

                    command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);

                    var queryResult = (int)command.ExecuteScalar();

                    var result = queryResult == 1 ? true : false;

                    return result;
                }
            }
        }

            

            
        public bool VerifyExistingWarehouse(Warehouse warehouse)
        {
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {

                    //Sprawdz czy magazyn o podanym id istnieje 

                    command.Connection = connection;

                    command.CommandText = " IF EXISTS " +
                    "(select * from [ApbdAsync].[dbo].[Warehouse] where IdWarehouse = @IdWarehouse) " +
                    "BEGIN " +
                    " SELECT 1 " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "SELECT 2 " +
                    "END";

                    command.Parameters.AddWithValue("@IdWarehouse", warehouse.IdWarehouse);

                    var queryResult = (int)command.ExecuteScalar();

                    var result = queryResult == 1 ? true : false;

                    return result;

                }
            }
        }





        //Aktualizujemy kolumnę FullfilledAt zamówienia na aktualną datę

        public void UpdateFullfilledAt(DateTime createdAt, decimal orderId)  
        {

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                connection.Open();


                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "UPDATE [ApbdAsync].[dbo].[Order] set FulfilledAt = @IdProduct where IdOrder = @orderId";

                    command.Parameters.AddWithValue("@createdAt", createdAt);
                    command.Parameters.AddWithValue("@orderId", orderId);

                    var result = (int)command.ExecuteScalar();

                    Console.WriteLine("UpdateFullfilledAt executed");
                }
            }

        }





        public decimal InsertNewOrder(Warehouse warehouse)
        {
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "INSERT INTO[ApbdAsync].[dbo].[Order]([IdProduct], [Amount], [CreatedAt], [FulfilledAt]) " +
                        "VALUES(@IdProduct, @Amount, @CreatedAt, null); " +
                        "SELECT SCOPE_IDENTITY()";

                    command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);
                    command.Parameters.AddWithValue("@Amount", warehouse.Amount);
                    command.Parameters.AddWithValue("@CreatedAt", warehouse.CreatedAt);

                    var orderIdentity = (decimal)command.ExecuteScalar();

                    Console.WriteLine("123123: " + orderIdentity);

                    UpdateFullfilledAt(warehouse.CreatedAt, orderIdentity);


                    command.CommandText = "select [Price] from [ApbdAsync].[dbo].[Product] where [IdProduct] = @IdProduct";

                    command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);

                    var productPrice = (decimal)command.ExecuteScalar();

                      

                    command.CommandText = "INSERT INTO [ApbdAsync].[dbo].[Product_Warehouse] ([IdWarehouse],[IdProduct],[IdOrder],[Amount],[Price],[CreatedAt]) " +
                        "VALUES (@IdWarehouse, @IdProduct,@IdOrder,@Amount,@Price,@CreatedAt); " +
                        "SELECT SCOPE_IDENTITY()";

                    command.Parameters.AddWithValue("@IdWarehouse", warehouse.IdWarehouse);
                    command.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);
                    command.Parameters.AddWithValue("@IdOrder", orderIdentity);
                    command.Parameters.AddWithValue("@Amount", warehouse.Amount);
                    command.Parameters.AddWithValue("@Price", warehouse.Amount * productPrice);
                    command.Parameters.AddWithValue("@CreatedAt", warehouse.CreatedAt);

                    //Ma zwrócić idProduct_Warehouse

                    var idProductWarehouse = (decimal)command.ExecuteScalar();

                    return idProductWarehouse;
                }
            }   
        }
    }
}
