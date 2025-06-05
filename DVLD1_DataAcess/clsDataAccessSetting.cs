using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD1_DataAcess
{
    internal class clsDataAccessSetting
    {
        //public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=sa123456;";
       
        static public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            }
        }
    }
}
