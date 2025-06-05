using DVLD1_DataAcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_Business
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int ID { get; set; }
        public string Title { get; set; }
        public float Fees { get; set; }
        

        clsApplicationType(int ID, string Title, float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
            Mode = enMode.Update;
        }
        public clsApplicationType()
        {
            this.ID = 0;
            this.Title = "";
            this.Fees = 0;

            Mode = enMode.AddNew;

        }

        public static DataTable GetApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicatonTypes();
        }
      
        static public clsApplicationType Find(int ApplicationTypeID)
        {
            bool IsFound;
            string Title = "";
            float Fees = 0;

            IsFound = clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID,ref Title, ref Fees);

            
            if (IsFound)
            {
                return new clsApplicationType(ApplicationTypeID, Title, Fees);
            }
            else { return null; }
        }
       

        private bool _AddNewApplicationType()
        {

            ID = clsApplicationTypeData.AddNewApplicationType(Title, Fees);

            return ID != -1;
        }

        private bool _UpdateApplicationType()
        {

            return clsApplicationTypeData.UpdateApplicationType(ID, Title, Fees);


        }
       

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    if (_UpdateApplicationType())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default
                    :
                    return false;
            }
        }
    }
}
