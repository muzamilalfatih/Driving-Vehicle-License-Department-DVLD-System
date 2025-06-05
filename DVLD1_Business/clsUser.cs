using DVLD1_DataAcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD1_Business
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool IsActive { get; set; }

        clsUser(int UserID, int PersonID, string UserName, string PassWord, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.PassWord = PassWord;
            this.IsActive = IsActive;
            Mode = enMode.Update;
        }

        public clsUser()
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.PassWord = PassWord;
            this.IsActive = IsActive;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }
        public static bool IsUserExistByPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }


        static public clsUser Find(int UserID)
        {
            bool IsFound;
            int PersonID = 0;
            string UserName = "", PassWord = "";
            bool IsActive = false;


            IsFound = clsUserData.GetUeserInfoByUserID(UserID,ref PersonID, ref UserName, ref PassWord, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, PassWord, IsActive);
            }
            else { return null; }
        }
        static public clsUser Find(string UserName, string PassWord)
        {
            bool IsFound = false;
            int UserID = 0;
            int PersonID = 0;
            bool IsActive = false;


            IsFound = clsUserData.GetUserInfoByUserNameAndPassWord(UserName, PassWord, ref UserID, ref PersonID, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, PassWord, IsActive);
            }
            else { return null; }
        }
        static public clsUser FindByPersonID(int PersonID)
        {

            bool IsFound = false;
            int UserID = 0;
            string UserName ="";
            string PassWord = "";
            bool IsActive = false;


            IsFound = clsUserData.GetUeserInfoByPersonID(PersonID,ref UserID,ref UserName, ref PassWord, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, PassWord, IsActive);
            }
            else { return null; }
        }

        private bool _AddNewUser()
        {

            UserID = clsUserData.AddNewUser(PersonID, UserName, PassWord, IsActive);

            return UserID != -1;
        }

        private bool _UpdateUser()
        {

            return clsUserData.UpdateUser(UserID, PersonID, UserName, PassWord, IsActive);


        }
         static public bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    if (_UpdateUser())
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
