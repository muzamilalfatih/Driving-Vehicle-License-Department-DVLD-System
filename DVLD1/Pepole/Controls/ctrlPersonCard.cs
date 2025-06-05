using DVLD1_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Pepole.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        public  int PersonID ;
        private clsPerson _Person;
        
        
        private string _GetFullName ()
        {
            return _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + _Person.LastName;
        }
        private void _LoadPersonImage()
        {
            if (_Person.ImagePath != "")
            {
                if (File.Exists(_Person.ImagePath))
                {
                    PersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                   
                    if (_Person.Gendor == 0)
                    {
                        PersonImage.ImageLocation = @"C:\Users\muzam\source\repos\DVLD1\Resources\Male 512.png";
                    }
                    else
                    {
                        PersonImage.ImageLocation = @"C:\Users\muzam\source\repos\DVLD1\Resources\Female 512.png";
                    }
                    
                }
            }
            else
            {
                if (_Person.Gendor == 0)
                {
                    PersonImage.ImageLocation = @"C:\Users\muzam\source\repos\DVLD1\Resources\Male 512.png";
                }
                else
                {
                    PersonImage.ImageLocation = @"C:\Users\muzam\source\repos\DVLD1\Resources\Female 512.png";
                }
            }
        }
        private  void _LoadPersonData()
        {
            lblPersonID.Text = PersonID.ToString();
            lblName.Text = _GetFullName();
            lblNationalNo.Text = _Person.NationalNo;
            if (_Person.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString();
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountries.GetCountryName(_Person.NationalityCountryID);
            _LoadPersonImage();
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
           
        }
        public bool LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person != null)
            {
                this.PersonID = _Person.PersonID;
                _LoadPersonData();
                llEditPersonInfo.Enabled = true;
                return true;
            }
            else
            {
                MessageBox.Show($"No person wiht ID {PersonID}");
                return false;
            }
        }
        public bool LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person != null)
            {
                this.PersonID = _Person.PersonID;
                _LoadPersonData();
                llEditPersonInfo.Enabled = true;
                return true;
            }
            else
            {
                MessageBox.Show($"No person wiht national no {NationalNo}");
                return false;
            }
        }
        private void ctrlPersonCard_Load(object sender, EventArgs e)
        {
            
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Form2_DataBack(object sender, int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            _LoadPersonData();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            frmAddNewUpdatePerson frm = new frmAddNewUpdatePerson(PersonID);
            frm.DataBack += Form2_DataBack; // Subscribe to the event
            frm.ShowDialog();
            LoadPersonInfo(_Person.PersonID);
        }
    }
}
