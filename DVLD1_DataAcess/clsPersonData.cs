using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace DVLD1_DataAcess
{
    public class clsPersonData
    {
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref short Gendor, ref string Address, ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath
            )
        {  
            if (IsPersonExist(PersonID))
            {
                SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
                string query = "Select * FROM People WHERE PersonID = @PersonID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        FirstName = (string)reader["FirstName"];
                        SecondName = (string)reader["SecondName"];                      
                        if (reader["ThirdName"] != DBNull.Value)
                        {
                            ThirdName = (string)reader["ThirdName"];
                        }
                        else
                        {
                            ThirdName = "";
                        }
                        LastName = (string)reader["LastName"];
                        NationalNo = (string)reader["NationalNo"];
                        DateOfBirth = (DateTime)reader["DateOfBirth"];
                        Gendor = (byte)reader["Gendor"];
                        Address = (string)reader["Address"];
                        Phone = (string)reader["Phone"];
                        if (reader["Email"] != DBNull.Value)
                        {
                            Email = (string)reader["Email"];
                        }
                        else
                        {
                            Email = "";
                        }
                        NationalityCountryID = (int)reader["NationalityCountryID"];
                        if (reader["ImagePath"] != DBNull.Value)
                        {
                            ImagePath = (string)reader["ImagePath"];
                        
                        }
                        else
                        {
                            ImagePath = "";
                        }
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
        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gendor, ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath)
        {
            if (IsPersonExist(NationalNo))
            {
                SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
                string query = "Select * FROM People WHERE NationalNo = @NationalNo";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        PersonID = (int)reader["PersonID"];
                        FirstName = (string)reader["FirstName"];
                        SecondName = (string)reader["SecondName"];
                        if (reader["ThirdName"] != DBNull.Value)
                        {
                            ThirdName = (string)reader["ThirdName"];
                        }
                        else
                        {
                            ThirdName = "";
                        }
                        LastName = (string)reader["LastName"];
                        NationalNo = (string)reader["NationalNo"];
                        DateOfBirth = (DateTime)reader["DateOfBirth"];
                        Gendor = (byte)reader["Gendor"];
                        Address = (string)reader["Address"];
                        Phone = (string)reader["Phone"];
                        if (reader["Email"] != DBNull.Value)
                        {
                            Email = (string)reader["Email"];
                        }
                        else
                        {
                            Email = "";
                        }
                        NationalityCountryID = (int)reader["NationalityCountryID"];
                        if (reader["ImagePath"] != DBNull.Value)
                        {
                            ImagePath = (string)reader["ImagePath"];

                        }
                        else
                        {
                            ImagePath = "";
                        }
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
            
        public static int AddNewPerson(string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"
                INSERT INTO People
           (
           FirstName
           ,SecondName
           ,ThirdName
           ,LastName
            ,NationalNo
           ,DateOfBirth
           ,Gendor
           ,Address
           ,Phone
           ,Email
           ,NationalityCountryID
           ,ImagePath)
     VALUES
           (
            @FirstName, 
           @SecondName, 
           @ThirdName, 
           @LastName, 
           @NationalNo,    
           @DateOfBirth, 
           @Gendor, 
           @Address, 
           @Phone, 
           @Email,
           @NationalityCountryID, 
           @ImagePath);
            SELECT SCOPE_IDENTITY();
                ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName != "" && ThirdName != null)
            {
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
            }
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            if (Email != "" && Email !=null)
            {
                command.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                command.Parameters.AddWithValue("@Email", DBNull.Value);
            }
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if (ImagePath != "" && ImagePath != null)
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            }
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int NewPersonID))
                {
                    PersonID = NewPersonID;
                }              
            }
            catch
            (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }
        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"Update  People  
                            set FirstName = @FirstName, 
                                SecondName = @SecondName, 
                                ThirdName = @ThirdName, 
                                LastName = @LastName, 
                                NationalNo = @NationalNo, 
                                DateOfBirth = @CountryID,                                
                                Gendor = @Gendor,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,                             
                                NationalityCountryID = @CountryID,
                                ImagePath = @ImagePath

                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            if (ThirdName != "" && ThirdName != null)
            {
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
            }
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            if (Email != "" && Email!= null)
            {
                command.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                command.Parameters.AddWithValue("@Email", DBNull.Value);
            }
            command.Parameters.AddWithValue("@CountryID", NationalityCountryID);
            if (ImagePath != "" && ImagePath != null)
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            }

            try
            {
                connection.Open();

                 rowsAffected = command.ExecuteNonQuery();               
            }
            catch (Exception ex)
            {
               
            }
            finally { 
                connection.Close(); 
            }

            return rowsAffected > 0;
        }
        public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName," +
                @"People.ThirdName, People.LastName, People.DateOfBirth, People.Gendor,
                CASE
                WHEN People.Gendor = 0 THEN 'Male'
                ELSE 'Female'
                END as GendorCaption ,
                People.Address, Countries.CountryName , People.Phone, People.Email
                FROM     People INNER JOIN
                Countries ON People.NationalityCountryID = Countries.CountryID";
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
                Console.WriteLine("Error : " +  ex.Message);
            }
            finally
            { 
                connection.Close(); 
            }
            return dt;
        }
        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);

            string query = @"DELETE FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
 
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
        public static bool IsPersonExist(int PersonID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "SELECT PersonID FROM People WHERE PersonID = @PersonID";
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
        public static bool IsPersonExist(string NationalNo)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "SELECT PersonID FROM People WHERE NationalNo = @NationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
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
        public static DateTime GetDateOfBith(int PersonID)
        {
            DateTime dt = DateTime.Now; 
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = @"select DateOFBirth from people 
                            where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dt = (DateTime)reader["DateOFBirth"];
                }
                
            }
            catch
            (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
                
            }
            return dt;
        }
    }
}
