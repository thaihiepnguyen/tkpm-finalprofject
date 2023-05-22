using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using MyShop.DTO;
using System.Diagnostics;
using System.Windows.Documents;
using MyShop.UI.MainPage.Pages;
using DocumentFormat.OpenXml.Office.Word;
using System.Windows;

namespace MyShop.DAO
{
    internal class UserDAO
    {
        DatabaseUtilitites db = DatabaseUtilitites.getInstance();
        public UserDTO GetOne(string username, string password)
        {
            var query = "SELECT * FROM [user] WHERE Username = @Username AND Password = @Password";

            var command = new SqlCommand(query, db.connection);
            AesHelper aesHelper = new AesHelper();
            string encryptedText = aesHelper.Encrypt(password);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", encryptedText);

            var reader = command.ExecuteReader();

            UserDTO user = null;
            if (reader.Read())
            {
                user = new UserDTO
                {
                    UserID = (int)reader["UserID"],
                    Username = (string)reader["Username"],
                    Password = (string)reader["Password"],
                    Fullname = (string)reader["Fullname"],
                    Gender = (string)reader["Gender"],
                    Address = (string)reader["Address"],
                    Tel = (string)reader["Tel"],
                    AvatarPath = (string)reader["AvatarPath"],
                    IsHide = (byte)reader["IsHide"],
                    RoleID = (int)reader["RoleID"]
                };
            }
            reader.Close();
            return user;
        }

        public bool CheckUserExist(string username)
        {
            var query = "SELECT * FROM [user] WHERE Username = @Username";

            var command = new SqlCommand(query, db.connection);
            AesHelper aesHelper = new AesHelper();
            command.Parameters.AddWithValue("@Username", username);

            var result = command.ExecuteScalar();
            if (result != null)
            {
                return false;
            }
            
            return true;
        }

        public bool CreateUser(UserDTO user)
        {
            // Kiểm tra username tồn tại
            if(!CheckUserExist(user.Username))
            {
                return false;
            }

            var query = "INSERT INTO [user] (Username, Password, Fullname, Gender, Address, Tel, AvatarPath, IsHide, RoleID) VALUES (@Username, @Password, @Fullname, @Gender, @Address, @Tel, @AvatarPath, @IsHide, @RoleID)";

            var command = new SqlCommand(query, db.connection);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Fullname", user.Fullname);
            command.Parameters.AddWithValue("@Gender", user.Gender);
            command.Parameters.AddWithValue("@Address", user.Address);
            command.Parameters.AddWithValue("@Tel", user.Tel);
            command.Parameters.AddWithValue("@AvatarPath", user.AvatarPath);
            command.Parameters.AddWithValue("@IsHide",user.IsHide);
            command.Parameters.AddWithValue("@RoleID", user.RoleID);

            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
