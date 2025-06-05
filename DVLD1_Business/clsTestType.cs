using DVLD1_DataAcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_Business
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public clsTestType.enTestType ID { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public float Fees { set; get; }
        public clsTestType()

        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public clsTestType(clsTestType.enTestType ID, string TestTypeTitel, string Description, float TestTypeFees)

        {
            this.ID = ID;
            this.Title = TestTypeTitel;
            this.Description = Description;

            this.Fees = TestTypeFees;
            Mode = enMode.Update;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        static public clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            bool IsFound;
            
            string TestTypeTitle = "";
            string TestTypeDescription = "";
            float TestTypeFees = 0;
                
            IsFound = clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID,ref TestTypeTitle,ref TestTypeDescription, ref TestTypeFees);

            

            if (IsFound)
            {
                return new clsTestType(TestTypeID,TestTypeTitle,TestTypeDescription, TestTypeFees); 
            }
            else { return null; }
        }


        private bool _AddNewApplicationType()
        {

            ID = (enTestType)clsTestTypeData.AddNewTestType(Title, Description, Fees);

            return (int) ID != -1;
        }

        private bool _UpdateApplicationType()
        {

            return clsTestTypeData.UpdateTestType((int)ID, Title, Description, Fees);


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
