using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_DataAcess
{
    public  class clsUserData
    {
        static public bool GetUeserInfoByUserID(int UserID, ref int PersonID, ref string UserName,
            ref string Password, ref bool IsActive)
        {
            if (IsUserExist(UserID))
            {
                SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
                string query = "Select * FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", UserID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        PersonID = (int)reader["PersonID"];
                        UserName = (string)reader["UserName"];
                        Password = (string)reader["Password"];

                        IsActive = (bool)reader["IsActive"];    
                    }
                    return true;
                }
                catch
                (Exception ex)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
            else
            {
                return false;
            }
        }
        static public bool GetUeserInfoByPersonID(int PersonID, ref int UserID, ref string UserName,
            ref string Password, ref bool IsActive)
        {
            if (IsUserExistForPersonID(PersonID))
            {
                SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
                string query = "Select * FROM Users WHERE PersonID = @PersonID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        UserID = (int)reader["UserID"];
                        UserName = (string)reader["UserName"];
                        Password = (string)reader["Password"];
                        IsActive = (bool)reader["IsActive"];
                    }
                    return true;
                }
                catch
                (Exception ex)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }

            }
            else
            {
                return false;
            }
            return false;
        }
        static public bool GetUserInfoByUserNameAndPassWord(string UserName, string PassWord,ref int UserID, ref int PersonID, ref bool IsActive) 
        {
            bool IsFound = false;

            if (IsUserExist( UserName))
            {
                SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
                string query = "select * from users where UserName = @UserName and Password = @PassWord";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@PassWord", PassWord);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        UserID = (int)reader["UserID"];
                        PersonID = (int)reader["PersonID"];
                        IsActive = (bool)reader["IsActive"];
                        IsFound = true;
                    }
                    
                }
                catch
                (Exception ex)
                {
                    return IsFound;
                }
                finally
                {
                    connection.Close();
                }
                return IsFound;
            }
            else
            {
                return IsFound;
            }
        }
        static public int AddNewUser(int PersonID, string UserName,string Password, bool IsActive)
        {
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"
                INSERT INTO Users
           (
           PersonID
           ,UserName
           ,Password
           ,IsActive
            )
     VALUES
           (
            @PersonID, 
           @UserName, 
           @Password, 
           @IsActive 
           );
            SELECT SCOPE_IDENTITY();
                ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int NewUserID))
                {
                    UserID = NewUserID;
                }
            }
            catch
            (Exception ex)
            {
                return UserID;
            }
            finally
            {
                connection.Close();
            }
            return UserID;
        }
        static public bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"Update  Users  
                            set PersonID = @PersonID, 
                                UserName = @UserName, 
                                Password = @Password, 
                                IsActive = @IsActive 

                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);   
            command.Parameters.AddWithValue("@IsActive", IsActive);
           
            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        static public bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"DELETE FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        static public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"SELECT Users.UserID, Users.PersonID, 
                            FullName = People.FirstName +' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName , Users.UserName,
                             Users.IsActive
                            FROM     Users INNER JOIN
                            People ON Users.PersonID = People.PersonID ";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        static public  bool IsUserExist(int UserID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "SELECT PersonID FROM Users WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsFound = reader.HasRows;
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;
        }
        static public bool IsUserExist(string UserName)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "SELECT Found = 1 FROM Users WHERE UserName = @UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsFound = reader.HasRows;
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;
        }
        static public bool IsUserExistForPersonID(int PersonID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "SELECT PersonID FROM Users WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsFound = reader.HasRows;
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }


            return IsFound;
        }

    }
}
