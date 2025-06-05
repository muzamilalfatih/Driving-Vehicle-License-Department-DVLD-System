using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_DataAcess
{
    public  class clsCountriesData
    {
        static public DataTable GetAllCountries ()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "select CountryName from Countries";
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
                clsEventLog.LogError(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        static public int GetCountryNationalNumber(string CountryName)
        {
            int CountryNationalNumber = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "Select CountryID from countries where countryname = @CountryName;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryNationalNumber = (int)reader["CountryID"];
                }
               
            }
            catch
            (Exception ex)
            {
                clsEventLog.LogError(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return CountryNationalNumber;
        }
        
        static public string GetCountryName(int CountryID)

        {
            string CountryName = "";

            SqlConnection connection = new SqlConnection(clsDataAccessSetting.ConnectionString);
            string query = "Select CountryName from countries where CountryID = @CountryID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryName = (string)reader["CountryName"];
                }

            }
            catch
            (Exception ex)
            {
                clsEventLog.LogError(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return CountryName;
        }
            
        }
    }

