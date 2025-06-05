using DVLD1.Global_Classes;
using DVLD1_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Users
{
   

    public partial class frmAddUpdateUser : Form
    {

        public delegate void DataBackEventHandler(object sender, int UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _UserID = -1;
        clsUser _User;

        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();

                tcLoginInfo.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tcLoginInfo.Enabled = true;
                btnSave.Enabled = true;


            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;


        }
        private void _LoadUserData()
        {
            
            _User = clsUser.Find(_UserID);
            ctrlPersonCardWithFilter1.LoadPersonData(_User.PersonID);
            lblUserID.Text = _UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
        }
        public frmAddUpdateUser()
        {
            InitializeComponent();
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode = enMode.Update;
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tcLoginInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tcLoginInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if (clsUser.IsUserExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tcLoginInfo.Enabled = true;
                    tabControl1.SelectedTab = tabControl1.TabPages["tcLoginInfo"];
                }
                
                
            }
            else
            {
                MessageBox.Show("Please select a person!", "Select A Person",MessageBoxButtons.OK);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
             if (_Mode == enMode.Update)
                _LoadUserData();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
           
                if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "Username cannot be blank");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                };

                if (_Mode == enMode.AddNew)
                {
                    if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }
                }
                else
                {
                    if (_User.UserName != txtUserName.Text.Trim())
                    {
                        if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                        {
                            e.Cancel = true;
                            errorProvider1.SetError(txtUserName, "username is used by another user");
                            return;
                        }
                        else
                        {
                            errorProvider1.SetError(txtUserName, null);
                        };
                    }
                }
            
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };
        }


        private void tabControl1_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(this, _User.UserID);
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.PassWord = clsGlobal.ComputeHash(txtPassword.Text.Trim());
            _User.IsActive = chkIsActive.Checked;

            if (_User.Save())
            {
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                lblUserID.Text = _User.UserID.ToString();
                MessageBox.Show("Data saved successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        

        private void frmAddUpdateUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this,_User.UserID);
            this.Close();
        }

        
    }
}
