using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyShop.DAO
{
    class OrderDAO
    {

        DatabaseUtilitites db = DatabaseUtilitites.getInstance();

        public ObservableCollection<OrderDTO> getOrders(int userID)
        {
            ObservableCollection<OrderDTO> list = new ObservableCollection<OrderDTO>();
            string query = "SELECT * FROM Orders WHERE UserID = @UserID";


            var command = new SqlCommand(query, db.connection);
            command.Parameters.AddWithValue("@UserID", userID);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                OrderDTO order = new OrderDTO
                {
                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                    ProID = reader.GetInt32(reader.GetOrdinal("ProID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                    ProName = reader.GetString(reader.GetOrdinal("ProName")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                    DeliveryDate = reader.GetDateTime(reader.GetOrdinal("DeliveryDate"))
                };
                list.Add(order);
            }

            reader.Close();

            return list;
        }

        public OrderDTO GetOne(int OrderID)
        {
            var query = "SELECT * FROM Orders WHERE OrderID = @OrderID";

            var command = new SqlCommand(query, db.connection);
            command.Parameters.AddWithValue("@OrderID", OrderID);

            var reader = command.ExecuteReader();

            OrderDTO order = null;
            if (reader.Read())
            {
                order = new OrderDTO
                {
                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                    ProID = reader.GetInt32(reader.GetOrdinal("ProID")),
                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                    ProName = reader.GetString(reader.GetOrdinal("ProName")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                    DeliveryDate = reader.GetDateTime(reader.GetOrdinal("DeliveryDate"))
                };
            }
            reader.Close();
            return order;
        }

        public bool DeleteOne(int OrderID)
        {
            Trace.WriteLine(OrderID);
            var query = "DELETE FROM Orders WHERE OrderID = @OrderIdToDelete";

            var command = new SqlCommand(query, db.connection);
            command.Parameters.AddWithValue("@OrderIdToDelete", OrderID);

            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        public bool UpdateOne(int OrderID, String newAddress)
        {
            Trace.WriteLine(newAddress);
            Trace.WriteLine(OrderID);
            var query = "UPDATE Orders SET Address = @NewAddress WHERE OrderID = @OrderID";

            var command = new SqlCommand(query, db.connection);
            command.Parameters.AddWithValue("@OrderID", OrderID);
            command.Parameters.AddWithValue("@NewAddress", newAddress);
            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        public bool CreateOrder(int proID, int userID, string address, DateTime orderDate, DateTime deliveryDate)
        {
            string query = "INSERT INTO Orders (ProID, UserID, Address, OrderDate, DeliveryDate) VALUES (@ProID, @UserID, @Address, @OrderDate, @DeliveryDate); SELECT SCOPE_IDENTITY()";

            var command = new SqlCommand(query, db.connection);
            command.Parameters.AddWithValue("@ProID", proID);
            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@OrderDate", orderDate);
            command.Parameters.AddWithValue("@DeliveryDate", deliveryDate);

            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public int insertShopOrder(ShopOrderDTO shopOrderDTO)
        {
            // insert SQL
            string sql = "insert into shop_order(CusID, CreateAt)" +
            "values(@CusID, @CreateAt)";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@CusID", SqlDbType.Int).Value = shopOrderDTO.CusID;
            command.Parameters.Add("@CreateAt", SqlDbType.Date).Value = shopOrderDTO.CreateAt;

            command.ExecuteNonQuery();

            // select SQL
            int id = -1;
            string sql1 = "SELECT TOP 1 OrderID FROM shop_order ORDER BY OrderID DESC ";

            var command1 = new SqlCommand(sql1, db.connection);

            var reader = command1.ExecuteReader();
            while (reader.Read())
            {
                id = (int)reader["OrderID"];
            }

            reader.Close();

            return id;
        }

        public void insertPurchase(PurchaseDTO purchaseDTO)
        {
            string sql = "insert into purchase(OrderID, ProID, Quantity, TotalPrice)" +
            "values(@OrderID, @ProID, @Quantity, @TotalPrice)";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@OrderID", SqlDbType.Int).Value = purchaseDTO.OrderID;
            command.Parameters.Add("@ProID", SqlDbType.Int).Value = purchaseDTO.ProID;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = purchaseDTO.Quantity;
            command.Parameters.Add("@TotalPrice", SqlDbType.Money).Value = purchaseDTO.TotalPrice;

            command.ExecuteNonQuery();

            // Xóa Quantity ở sản phẩm ProID

            ProductBUS productBUS = new ProductBUS();

            var product = productBUS.findProductById(purchaseDTO.ProID);

            product.Quantity = product.Quantity - purchaseDTO.Quantity;

            productBUS.patchProduct(product);
        }

        public void updateShopOrder(ShopOrderDTO shopOrder)
        {
            string sql = "update shop_order " +
                "set CusID =  @CusID, CreateAt = @CreateAt, FinalTotal = @FinalTotal, ProfitTotal = @ProfitTotal " +
                "where OrderID = @OrderID";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@OrderID", SqlDbType.Int).Value = shopOrder.OrderID;
            command.Parameters.Add("@CusID", SqlDbType.Int).Value = shopOrder.CusID;
            command.Parameters.Add("@CreateAt", SqlDbType.Date).Value = shopOrder.CreateAt;
            command.Parameters.Add("@FinalTotal", SqlDbType.Money).Value = shopOrder.FinalTotal;
            command.Parameters.Add("@ProfitTotal", SqlDbType.Money).Value = shopOrder.ProfitTotal;

            command.ExecuteNonQuery();
        }

        public async Task<ObservableCollection<ShopOrderDTO>> getAll()
        {
            ObservableCollection<ShopOrderDTO> list = new ObservableCollection<ShopOrderDTO>();

            await Task.Run(() =>
            {
                string sql = "select OrderID, CusID, CreateAt, FinalTotal, ProfitTotal from shop_order where FinalTotal > 0";

                var command = new SqlCommand(sql, db.connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ShopOrderDTO shopOrder = new ShopOrderDTO
                    {
                        OrderID = (int)reader["OrderID"],
                        CusID = reader["CusID"] == DBNull.Value ? null : (int?)reader["CusID"],
                        CreateAt = (DateTime)reader["CreateAt"],
                        // này để tránh lỗi :)
                        FinalTotal = reader["FinalTotal"] == DBNull.Value ? null : (decimal?)reader["FinalTotal"],
                        ProfitTotal = reader["ProfitTotal"] == DBNull.Value ? null : (decimal?)reader["ProfitTotal"]
                    };

                    if (shopOrder.CusID != null)
                    {
                        list.Add(shopOrder);
                    }   
                }

                reader.Close();
                System.Threading.Thread.Sleep(500);
            });

            return list;
        }

        public int countTotalOrder()
        {
            int total = 0;
            string sql = $"""
                select count(*) as sum from shop_order
                """;

            var command = new SqlCommand(sql, db.connection);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                total = (int)reader["sum"];
            }

            reader.Close();

            return total;
        }

        public List<PurchaseDTO> getPurchaseDTOs(int id)
        {
            List<PurchaseDTO> list = new List<PurchaseDTO>();

            string sql = "select PurchaseID, OrderID, ProID, Quantity, TotalPrice " +
            "from purchase where OrderID = @id";

            var command = new SqlCommand(sql, db.connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PurchaseDTO purchase= new PurchaseDTO();
                purchase.PurchaseID = (int)reader["PurchaseID"];
                purchase.OrderID = (int)reader["OrderID"];
                purchase.ProID = (int)reader["ProID"];
                purchase.Quantity = (int)reader["Quantity"];
                purchase.TotalPrice = (decimal)reader["TotalPrice"];

                list.Add(purchase);
            }

            reader.Close();
            return list;
        }

        public void deletePurchaseById(int id)
        {
            string sql = $"""
                delete purchase 
                where PurchaseID = {id}
                """;

            var command = new SqlCommand(sql, db.connection);

            command.ExecuteNonQuery();
        }

        public void deleteOrderById(int id)
        {
            // Xóa ở bảng purchase
            var purchases = getPurchaseDTOs(id);
            foreach (var purchase in purchases)
            {
                deletePurchaseById(purchase.PurchaseID);
            }
            // Xóa ở bảng shop_order
            string sql = $"""
                delete shop_order 
                where OrderID = {id}
                """;

            var command = new SqlCommand(sql, db.connection);

            command.ExecuteNonQuery();
        }

        // lấy những purchase mà cusid đã mua
        public List<PurchaseDTO> getPuchasesbyCusID(int id)
        {
            List<PurchaseDTO> list = new List<PurchaseDTO>();
            string sql = "select PurchaseID, OrderID, ProID, Quantity, TotalPrice from purchase as p where p.OrderID = " +
                "(select s.OrderID from shop_order as s where CusID = @id and p.OrderID = s.OrderID)";
            var command = new SqlCommand(sql, db.connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PurchaseDTO purchase = new PurchaseDTO();
                purchase.PurchaseID = (int)reader["PurchaseID"];
                purchase.OrderID = (int)reader["OrderID"];
                purchase.ProID = (int)reader["ProID"];
                purchase.Quantity = (int)reader["Quantity"];
                purchase.TotalPrice = (decimal)reader["TotalPrice"];

                list.Add(purchase);
            }

            reader.Close();
            return list;
        }
    }
}