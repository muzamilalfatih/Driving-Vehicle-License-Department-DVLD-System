using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DVLD1_DataAcess;


namespace DVLD1_Business
{
   
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
       

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }

        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { get; set; }
        public string GendorText()
        {
            return Gendor == 0 ? "Male" : "Female";
        }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, short Gendor, string Address,
            string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

        }
         public clsPerson()
        {
            this.PersonID = -1;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

        }
        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }
        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
        static public clsPerson Find(int PersonID)
        {
            bool IsFound;
            string FirstName ="", SecondName = "", ThirdName = "", LastName = "", NationalNo = "", ImagePath = "",
                 Address = "", Phone = "", Email = "";
            DateTime DateOfBirth = DateTime.Now; 
            short Gendor =0; 
            int NationalityCountryID = 0;

            IsFound = clsPersonData.GetPersonInfoByID(PersonID, ref FirstName, ref SecondName,
          ref ThirdName, ref LastName, ref NationalNo, ref DateOfBirth,
           ref Gendor, ref Address, ref Phone, ref Email,
           ref NationalityCountryID, ref ImagePath);
            if (IsFound)
            {
                return new  clsPerson(PersonID, FirstName, SecondName,
           ThirdName, LastName, NationalNo, DateOfBirth,
            Gendor, Address, Phone, Email,
            NationalityCountryID, ImagePath);
            }
            else { return null; }
        }
        static public clsPerson Find(string NationalNO)
        {
            bool IsFound;
            int PersonID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", NationalNo = "", ImagePath = "",
                 Address = "", Phone = "", Email = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = 0;
            int NationalityCountryID = 0;

            IsFound = clsPersonData.GetPersonInfoByNationalNo(NationalNO, ref PersonID, ref FirstName, ref SecondName,
          ref ThirdName, ref LastName,  ref DateOfBirth,
           ref Gendor, ref Address, ref Phone, ref Email,
           ref NationalityCountryID, ref ImagePath);

            if (IsFound)
            {
                return new clsPerson(PersonID, FirstName, SecondName,
           ThirdName, LastName, NationalNo, DateOfBirth,
            Gendor, Address, Phone, Email,
            NationalityCountryID, ImagePath);
            }
            else { return null; }
        }

        private bool _AddNewPerson ()
        {

             PersonID =  clsPersonData.AddNewPerson( FirstName,  SecondName,
            ThirdName,  LastName,  NationalNo,  DateOfBirth,
            Gendor,  Address,  Phone,  Email,
             NationalityCountryID,  ImagePath);

            return PersonID != -1;
        }

        private bool _UpdatePerson()
        {

            return  clsPersonData.UpdatePerson(PersonID, FirstName, SecondName,
           ThirdName, LastName, NationalNo, DateOfBirth,
           Gendor, Address, Phone, Email,
            NationalityCountryID, ImagePath);

            
        }
        static public bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }
        static public DateTime GetDateOfBirth(int PersonID)
        {
            return clsPersonData.GetDateOfBith(PersonID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    if (_UpdatePerson())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    default
                    : return false;
            }
        }
    }
}
