using Microsoft.Data.SqlClient;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Collections.ObjectModel;

namespace MyShop.DAO
{
    class CategoryDAO
    {
        DatabaseUtilitites db = DatabaseUtilitites.getInstance();

        public ObservableCollection<CategoryDTO> getAll()
        {
            ObservableCollection<CategoryDTO> list = new ObservableCollection<CategoryDTO>();

            string sql = "select CatID, CatName, CatIcon, CatDescription from category";

            var command = new SqlCommand(sql, db.connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                CategoryDTO category = new CategoryDTO();
                category.CatID = (int)reader["CatID"];
                category.CatName = reader["CatName"] == DBNull.Value ? "Lỗi tên thể loại" : (string?)reader["CatName"];
                category.CatIcon = (string)reader["CatIcon"];
                category.CatDescription = (string)reader["CatDescription"];

                list.Add(category);
            }



            reader.Close();

            return list;
        }
        public CategoryDTO getCategoryById(int id)
        {
            List<CategoryDTO> list = new List<CategoryDTO>();
            CategoryDTO result = new CategoryDTO();

            string sql = $"select CatID, CatName, CatIcon, CatDescription from category where CatID = @id";

            var command = new SqlCommand(sql, db.connection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            SqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();
            } catch (Exception ex) { Console.WriteLine(ex.Message); }

            if (reader != null)
            {
                while (reader.Read())
                {
                    CategoryDTO category = new CategoryDTO();

                    category.CatID = (int)reader["CatID"];
                    category.CatName = reader["CatName"] == DBNull.Value ? "Lỗi tên thể loại" : (string?)reader["CatName"];
                    category.CatIcon = (string)reader["CatIcon"];
                    category.CatDescription = (string)reader["CatDescription"];

                    list.Add(category);
                }

                reader.Close();
                result = list[0];
            }

            return result;
        }

        public int insertCategory(CategoryDTO category)
        {
            string sql = "insert into category(CatName, CatIcon, CatDescription)" +
                "values(@CatName, @CatIcon, @CatDescription)";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@CatName", SqlDbType.NVarChar).Value = category.CatName;
            command.Parameters.Add("@CatIcon", SqlDbType.NVarChar).Value = category.CatIcon;
            command.Parameters.Add("@CatDescription", SqlDbType.NVarChar).Value = category.CatDescription;

            command.ExecuteNonQuery();

            // select SQL
            int id = -1;
            string sql1 = "SELECT TOP 1 CatID FROM category ORDER BY CatID DESC ";

            var command1 = new SqlCommand(sql1, db.connection);

            var reader = command1.ExecuteReader();
            while (reader.Read())
            {
                id = (int)reader["CatID"];
            }

            reader.Close();

            return id;
        }

        public Tuple<Boolean, string> delCategoryById(int catID)
        {
            string message = "";
            bool isSuccess = true;
            Tuple<Boolean, string> result = new Tuple<bool, string>(isSuccess, message);
            
            string sql = $"""
                delete category 
                where CatID = {catID}
                """;

            var command = new SqlCommand(sql, db.connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch(SqlException ex) {
                message = ex.Message;
                isSuccess = false;
                return new Tuple<bool, string>(isSuccess, message);
            }

            return result;
        }

        public void updateCategory(CategoryDTO category)
        {
            string sql = "update category " +
                "set CatName =  @CatName, CatIcon = @CatIcon, CatDescription = @CatDescription " +
                "where CatID = @CatID";
            var command = new SqlCommand(sql, db.connection);

            command.Parameters.Add("@CatID", SqlDbType.Int).Value = category.CatID;
            command.Parameters.Add("@CatName", SqlDbType.NVarChar).Value = category.CatName;
            command.Parameters.Add("@CatIcon", SqlDbType.NVarChar).Value = category.CatIcon;
            command.Parameters.Add("@CatDescription", SqlDbType.NVarChar).Value = category.CatDescription;

            command.ExecuteNonQuery();
        }
    }
}
