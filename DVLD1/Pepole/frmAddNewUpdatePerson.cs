using DVLD1.classes;
using DVLD1_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD1.Global_Classes;
using System.Reflection.Emit;
using DVLD1.Properties;

namespace DVLD1.Pepole
{
    public partial class frmAddNewUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private int _PersonID = -1;
        clsPerson _Person;
        private enum enMode  {addNew =0 ,Update = 1 };
        private enMode _Mode;
        private void _CheckValidation (object sender, CancelEventArgs e)
        {
            TextBox textBox =  (TextBox)sender;
            if (string.IsNullOrWhiteSpace((textBox.Text)))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "This field is required!");
            }
            else
            {
                e.Cancel= false;
                errorProvider1.SetError(textBox, "");
                if (textBox == txtNationalNo)
                {
                    if (clsPerson.IsPersonExist(txtNationalNo.Text) && txtNationalNo.Text != _Person.NationalNo)
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(textBox, "This national number is user by another person!");
                    }
                    else
                    {
                        e.Cancel = false;
                        errorProvider1.SetError(textBox, "");
                    } 
                }
            }
            

        }
        
        private void _LoadPersonData ()
        {
            if (_Person != null)
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                txtFirstName.Text = _Person.FirstName;
                txtSecondName.Text = _Person.SecondName;
                txtThirdName.Text = _Person.ThirdName;
                txtLastName.Text = _Person.LastName;
                txtNationalNo.Text = _Person.NationalNo;
                dtpDateOfBirth.Value = _Person.DateOfBirth;
                if (_Person.Gendor == 0)
                {
                    rbMale.Checked = true;
                }
                else
                {
                    rbFemale.Checked = true;
                }
                txtPhone.Text = _Person.Phone;
                txtEmail.Text = _Person.Email;
                //comboBox1.SelectedIndex = 
                txtAddress.Text = _Person.Address;
                if (_Person.ImagePath != "")
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }

                llRemoveImage.Visible = _Person.ImagePath != "";

            }
        }
        private void _FillComboBoxWithCountries()
        {
            DataTable dt = clsCountries.GetAllCountries();
            
            foreach (DataRow row in dt.Rows)
            {
                comboBox1.Items.Add(row["CountryName"].ToString());
            }
            comboBox1.SelectedItem = "Jordan";
        }
        public frmAddNewUpdatePerson()
        {
            InitializeComponent();
            _FillComboBoxWithCountries();
            
            _Mode = enMode.addNew;
        }
        public frmAddNewUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _FillComboBoxWithCountries();
            _PersonID = PersonID;
            _Mode = enMode.Update;
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                return;
            }
            if (!clsValidatoin.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid email address format");
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
                // ...
            }
        }
        private bool _HandlePersonImage()
        {
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch
                    {

                    }
            
                } 
                if (pbPersonImage.ImageLocation != null)
                {
                    string SourceImage = pbPersonImage.ImageLocation.ToString();
                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImage))
                    {
                        pbPersonImage.ImageLocation = SourceImage;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return false;
        }

        private void frmAddNewUpdatePerson_Load(object sender, EventArgs e)
        {
            _Person = new clsPerson();
            if (_Mode == enMode.addNew)
            {
                label1.Text = "Add New Person";
                llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

                dtpDateOfBirth.MaxDate = DateTime.Now;
            }
                

            else
            {
                label1.Text = "Update Person";
                _Person = clsPerson.Find(_PersonID);
                _Person.Mode = clsPerson.enMode.Update;
                _LoadPersonData();
            }
             
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
  
            if (!this.ValidateChildren())
            {
                
                MessageBox.Show("Som fields are not valide; put the mouse over the red icon(s) to see the erro",
                                "Validation Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);  
                return;
            }
            if (_HandlePersonImage())
            {
                return;
            }

            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            if (rbMale.Checked) _Person.Gendor = 0;
            else _Person.Gendor = 1;
            _Person.Phone = txtPhone.Text;
            _Person.NationalityCountryID = clsCountries.GetNationalCountryID(comboBox1.Text.ToString());
            _Person.Address = txtAddress.Text;
            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";

            

            if (_Person.Save())
            {
                _Person.Mode = clsPerson.enMode.Update;
                _Mode =enMode.Update;
                label1.Text = "Update Person";
                lblPersonID.Text = _Person.PersonID.ToString();

                MessageBox.Show("Data saved successfully", "Saved", MessageBoxButtons.OK,MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);

            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(this, _Person.PersonID);
            this.Close();
        }

        private void frmAddNewUpdatePerson_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this, _Person.PersonID);
        }

        
        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (_Person.Gendor == 0)
            {
                pbPersonImage.Image = Resources.Male_512;
                
            }
                
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }
                
            llRemoveImage.Visible = false;
        }

        private void llSetImage_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
                // ...
            }
        }
    }
}
