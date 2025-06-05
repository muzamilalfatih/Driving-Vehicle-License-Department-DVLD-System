using DVLD1_DataAcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_Business
{
    public class clsCountries
    {
        public static int GetNationalCountryID(string countryName)
        {
            return clsCountriesData.GetCountryNationalNumber(countryName);
        }
        public static string GetCountryName(int countryID)
        {
            return clsCountriesData.GetCountryName(countryID);
        }
        static public DataTable GetAllCountries()
        {
            return clsCountriesData.GetAllCountries();
        }
    }
}
