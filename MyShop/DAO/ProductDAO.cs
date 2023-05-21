using Microsoft.Data.SqlClient;
using MyShop.DTO;
using MyShop.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace MyShop.DAO
{
    internal class ProductDAO
    {
        DatabaseUtilitites db = DatabaseUtilitites.getInstance();
        private ObservableCollection<PromotionDTO> _promotion = (new PromotionDAO()).getAll();
        public async Task<ObservableCollection<ProductDTO>> getAll()
        {
            ObservableCollection<ProductDTO> list = new ObservableCollection<ProductDTO>();

            await Task.Run(() =>
            {
                string sql = "select ProID, ProName, Ram, Rom, ScreenSize, TinyDes," +
    " FullDes, Price, ImagePath, Trademark," +
    "BatteryCapacity, CatID, Quantity,PromoID, PromotionPrice, Block from product where Block = 0";
                var command = new SqlCommand(sql, db.connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductDTO product = new ProductDTO();
                    product.ProId = (int)reader["ProID"];
                    product.ProName = reader["ProName"] == DBNull.Value ? "Lỗi tên sản phẩm" : (string?)reader["ProName"];
                    product.Ram = (double)reader["Ram"];
                    product.Rom = (int)reader["Rom"];
                    product.ScreenSize = (double)reader["ScreenSize"];
                    product.TinyDes = reader["TinyDes"] == DBNull.Value ? null : (string?)reader["TinyDes"];
                    product.FullDes = reader["FullDes"] == DBNull.Value ? null : (string?)reader["FullDes"];
                    product.Price = (decimal)reader["Price"];
                    product.ImagePath = reader["ImagePath"] == DBNull.Value ? "Assets/Images/sp/404.png" : (string?)reader["ImagePath"];
                    product.Trademark = reader["Trademark"] == DBNull.Value ? null : (string?)reader["Trademark"];
                    product.BatteryCapacity = (int)reader["BatteryCapacity"];
                    product.CatID = (int)reader["CatID"];
                    product.Quantity = (int)reader["Quantity"];
                    product.PromotionPrice = reader["PromotionPrice"] == DBNull.Value ? product.Price : (decimal?)reader["PromotionPrice"];
                    product.PromoID = reader["PromoID"] == DBNull.Value ? null : (int?)reader["PromoID"];
                    product.Block = (int)reader["Block"];
                    list.Add(product);
                }

                System.Threading.Thread.Sleep(1000);

                reader.Close();
            });


            return list;
        }


        // TODO: code này là dựa trên mã nguồn của Thầy .
        public ObservableCollection<ProductDTO> getProducts(int currentPage = 1, int rowsPerPage = 10,
                string keyword = "")
        {
            var list = new ObservableCollection<ProductDTO>();
            string sql = "select ProID, ProName, Ram, Rom, ScreenSize, TinyDes," +
                " FullDes, Price, ImagePath, Trademark," +
                "BatteryCapacity, CatID, PromoID  from product";

            var command = new SqlCommand(sql, db.connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                ProductDTO product = new ProductDTO();
                product.ProId = (int)reader["ProID"];
                product.ProName = reader["ProName"] == DBNull.Value ? "Lỗi tên sản phẩm" : (string?)reader["ProName"];
                product.Ram = (double)reader["Ram"];
                product.Rom = (int)reader["Rom"];
                product.ScreenSize = (double)reader["ScreenSize"];
                product.TinyDes = reader["TinyDes"] == DBNull.Value ? null : (string?)reader["TinyDes"];
                product.FullDes = reader["FullDes"] == DBNull.Value ? null : (string?)reader["FullDes"];
                product.Price = (decimal)reader["Price"];
                product.ImagePath = reader["ImagePath"] == DBNull.Value ? "Assets/Images/sp/404.png" : (string?)reader["ImagePath"];
                product.Trademark = reader["Trademark"] == DBNull.Value ? null : (string?)reader["Trademark"];
                product.BatteryCapacity = (int)reader["BatteryCapacity"];
                product.PromotionPrice = reader["PromotionPrice"] == DBNull.Value ? (decimal)reader["Price"] : (decimal?)reader["PromotionPrice"];
                product.CatID = (int)reader["CatID"];
                product.PromoID = reader["PromoID"] == DBNull.Value ? null : (int?)reader["PromoID"];

                list.Add(product);
            }

            reader.Close();

            return list;
        }


        //xóa nhưng thật sự là không có xóa :))) 
        public void deleteProductById(int ProId)
        {
            string sql = $"""
                update product 
                set Block = {1}

                where ProID = {ProId}
                """;

            var command = new SqlCommand(sql, db.connection);

            command.ExecuteNonQuery();
        }

        // insert và trả ra id
        public int insertProduct(ProductDTO productDTO)
        {
            // insert SQL
            string sql = "insert into product(ProName, Ram, Rom, ScreenSize, TinyDes, Price, PromotionPrice, Trademark, BatteryCapacity, CatID, Quantity,Block)" +
            "values(@ProName, @Ram, @Rom, @ScreenSize, @TinyDes, @Price, @PromotionPrice, @Trademark, @BatteryCapacity, @CatID, @Quantity,@Block)";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@ProName", SqlDbType.NVarChar).Value = productDTO.ProName;
            command.Parameters.Add("@Ram", SqlDbType.Float).Value = productDTO.Ram;
            command.Parameters.Add("@Rom", SqlDbType.Int).Value = productDTO.Rom;
            command.Parameters.Add("@ScreenSize", SqlDbType.Float).Value = productDTO.ScreenSize;
            command.Parameters.Add("@TinyDes", SqlDbType.NVarChar).Value = productDTO.TinyDes;
            command.Parameters.Add("@Price", SqlDbType.Money).Value = productDTO.Price;
            command.Parameters.Add("@PromotionPrice", SqlDbType.Money).Value = productDTO.PromotionPrice == null? productDTO.Price : productDTO.PromotionPrice;
            command.Parameters.Add("@Trademark", SqlDbType.Text).Value = productDTO.Trademark;
            command.Parameters.Add("@BatteryCapacity", SqlDbType.Int).Value = productDTO.BatteryCapacity;
            command.Parameters.Add("@CatID", SqlDbType.Int).Value = productDTO.CatID;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = productDTO.Quantity;
            command.Parameters.Add("@Block", SqlDbType.Int).Value = productDTO.Block;

            command.ExecuteNonQuery();

            // select SQL
            int id = -1;
            string sql1 = "SELECT TOP 1 ProID FROM product ORDER BY ProID DESC ";

            var command1 = new SqlCommand(sql1, db.connection);

            var reader = command1.ExecuteReader();
            while (reader.Read())
            {
                id = (int)reader["ProID"];
            }

            reader.Close();

            return id;
        }

        public void updateProduct(ProductDTO productDTO)
        {
            string sql = "update product " +
                "set ProName =  @ProName, Ram = @Ram, Rom = @Rom, ScreenSize = @ScreenSize, TinyDes = @TinyDes," +
                "Price = @Price, Trademark = @Trademark, BatteryCapacity = @BatteryCapacity," +
                "CatID = @CatID, Quantity = @Quantity, PromoID = @PromoID, PromotionPrice = @PromotionPrice, Block = @Block " +
                "where ProID = @ProID";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@ProID", SqlDbType.Int).Value = productDTO.ProId;
            command.Parameters.Add("@ProName", SqlDbType.NVarChar).Value = productDTO.ProName;
            command.Parameters.Add("@Ram", SqlDbType.Float).Value = productDTO.Ram;
            command.Parameters.Add("@Rom", SqlDbType.Int).Value = productDTO.Rom;
            command.Parameters.Add("@ScreenSize", SqlDbType.Float).Value = productDTO.ScreenSize;
            command.Parameters.Add("@TinyDes", SqlDbType.NVarChar).Value = productDTO.TinyDes;
            command.Parameters.Add("@Price", SqlDbType.Money).Value = productDTO.Price;
            command.Parameters.Add("@Trademark", SqlDbType.Text).Value = productDTO.Trademark;
            command.Parameters.Add("@BatteryCapacity", SqlDbType.Int).Value = productDTO.BatteryCapacity;
            command.Parameters.Add("@CatID", SqlDbType.Int).Value = productDTO.CatID;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = productDTO.Quantity;
            command.Parameters.Add("@PromotionPrice", SqlDbType.Money).Value = productDTO.PromotionPrice == null ? productDTO.Price : productDTO.PromotionPrice;
            command.Parameters.Add("@PromoID", SqlDbType.Int).Value = productDTO.PromoID == null ? DBNull.Value : productDTO.PromoID;
            command.Parameters.Add("@Block", SqlDbType.Int).Value = productDTO.Block;

            command.ExecuteNonQuery();
        }
        public void updateImage(int id, string key)
        {
            // update SQL
            string sql = $"""
                update product 
                set ImagePath = 'Assets/Images/sp/{key}.png'
                where ProID = {id}
                """;

            var command = new SqlCommand(sql, db.connection);

            command.ExecuteNonQuery();
        }

        public async Task<int> countTotalProduct()
        {
            int total = 0;
            await Task.Run(() =>
            {
                string sql = $"""
                select sum(Quantity) as sum from product
                """;

                var command = new SqlCommand(sql, db.connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    total = (int)reader["sum"];
                }

                System.Threading.Thread.Sleep(200);

                reader.Close();

                return total;
            });

            return total;
        }

        public ProductDTO getProductById(int id) 
        {
            List<ProductDTO> list = new List<ProductDTO>();
            ProductDTO result = new ProductDTO();

            string sql = "select ProID, ProName, Ram, Rom, ScreenSize, TinyDes," +
                " FullDes, Price, ImagePath, Trademark," +
                "BatteryCapacity, CatID, Quantity, PromoID, PromotionPrice, Block from product where ProID = @id";

            var command = new SqlCommand(sql, db.connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProductDTO product = new ProductDTO();
                product.ProId = (int)reader["ProID"];
                product.ProName = reader["ProName"] == DBNull.Value ? "Lỗi tên sản phẩm" : (string?)reader["ProName"];
                product.Ram = (double)reader["Ram"];
                product.Rom = (int)reader["Rom"];
                product.ScreenSize = (double)reader["ScreenSize"];
                product.TinyDes = reader["TinyDes"] == DBNull.Value ? null : (string?)reader["TinyDes"];
                product.FullDes = reader["FullDes"] == DBNull.Value ? null : (string?)reader["FullDes"];
                product.Price = (decimal)reader["Price"];
                product.ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string?)reader["ImagePath"];
                product.Trademark = reader["Trademark"] == DBNull.Value ? null : (string?)reader["Trademark"];
                product.BatteryCapacity = (int)reader["BatteryCapacity"];
                product.CatID = (int)reader["CatID"];
                product.PromoID = reader["PromoID"] == DBNull.Value ? null : (int?)reader["PromoID"];
                product.PromotionPrice = reader["PromotionPrice"] == DBNull.Value ? (decimal)reader["Price"] : (decimal?)reader["PromotionPrice"];
                product.Quantity = (int)reader["Quantity"];

                list.Add(product);
            }

            reader.Close();
            result = list[0];

            return result;
        }

        public async Task<ObservableCollection<ProductDTO>> getTop5Product()
        {
            ObservableCollection<ProductDTO> list = new ObservableCollection<ProductDTO>();
            await Task.Run(() =>
            {
                string sql = "select top 5 ProID, ProName, Ram, Rom, ScreenSize, TinyDes," +
    " FullDes, Price, ImagePath, Trademark," +
    "BatteryCapacity, CatID, Quantity,  PromoID,PromotionPrice, Block from product where Quantity <= 5 and Quantity > 0 order by Quantity asc";
                var command = new SqlCommand(sql, db.connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductDTO product = new ProductDTO();
                    product.ProId = (int)reader["ProID"];
                    product.ProName = reader["ProName"] == DBNull.Value ? "Lỗi tên sản phẩm" : (string?)reader["ProName"];
                    product.Ram = (double)reader["Ram"];
                    product.Rom = (int)reader["Rom"];
                    product.ScreenSize = (double)reader["ScreenSize"];
                    product.TinyDes = reader["TinyDes"] == DBNull.Value ? null : (string?)reader["TinyDes"];
                    product.FullDes = reader["FullDes"] == DBNull.Value ? null : (string?)reader["FullDes"];
                    product.Price = (decimal)reader["Price"];
                    product.ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string?)reader["ImagePath"];
                    product.Trademark = reader["Trademark"] == DBNull.Value ? null : (string?)reader["Trademark"];
                    product.BatteryCapacity = (int)reader["BatteryCapacity"];
                    product.CatID = (int)reader["CatID"];
                    product.Quantity = (int)reader["Quantity"];
                    product.PromoID = reader["PromoID"] == DBNull.Value ? null : (int?)reader["PromoID"];
                    product.PromotionPrice = reader["PromotionPrice"] == DBNull.Value ? (decimal)reader["Price"] : (decimal?)reader["PromotionPrice"];
                    product.Block = (int)reader["Block"];

                    list.Add(product);
                }
                System.Threading.Thread.Sleep(200);

                reader.Close();
            });



            return list;
        }

        public ObservableCollection<ProductDTO> getProductsByCusID(int CusID)
        {
            ObservableCollection<ProductDTO> list = new ObservableCollection<ProductDTO>();
            string sql = "select ProID, ProName, Ram, Rom, ScreenSize, TinyDes," +
                " FullDes, Price, ImagePath, Trademark," +
                " from product as pd where pd.ProID = \r\n" +
                "(select p.ProID from purchase as p where pd.ProID = p.ProID and p.OrderID = \r\n" +
                "(select s.OrderID from shop_order as s where CusID = @CusID and p.OrderID = s.OrderID))";

            var command = new SqlCommand(sql, db.connection);
            command.Parameters.Add("@CusID", SqlDbType.Int).Value = CusID;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProductDTO product = new ProductDTO();
                product.ProId = (int)reader["ProID"];
                product.ProName = reader["ProName"] == DBNull.Value ? "Lỗi tên sản phẩm" : (string?)reader["ProName"];
                product.Ram = (double)reader["Ram"];
                product.Rom = (int)reader["Rom"];
                product.ScreenSize = (double)reader["ScreenSize"];
                product.TinyDes = reader["TinyDes"] == DBNull.Value ? null : (string?)reader["TinyDes"];
                product.FullDes = reader["FullDes"] == DBNull.Value ? null : (string?)reader["FullDes"];
                product.Price = (decimal)reader["Price"];
                product.ImagePath = reader["ImagePath"] == DBNull.Value ? null : (string?)reader["ImagePath"];
                product.Trademark = reader["Trademark"] == DBNull.Value ? null : (string?)reader["Trademark"];
                product.BatteryCapacity = (int)reader["BatteryCapacity"];
                product.CatID = (int)reader["CatID"];
                product.PromoID = reader["PromoID"] == DBNull.Value ? null : (int?)reader["PromoID"];
                product.PromotionPrice = reader["PromotionPrice"] == DBNull.Value ? (decimal)reader["Price"] : (decimal?)reader["PromotionPrice"];
                product.Quantity = (int)reader["Quantity"];

                list.Add(product);
            }
            reader.Close();
            return list;
        }
    }
}
