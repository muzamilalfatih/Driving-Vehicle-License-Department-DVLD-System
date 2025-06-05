using DVLD1.Global_Classes;
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

namespace DVLD1.Users
{
    public partial class frmChangePassword : Form
    {

        public delegate void DataBackEventHandler(object sender, int UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public int UserID;
        clsUser _User;
        public frmChangePassword(int userID)
        {
            InitializeComponent();
            UserID = userID;
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            };

            if (_User.PassWord != txtCurrentPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            };
        }

        private void txtNewPasword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPasword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPasword, "New Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtNewPasword, null);
            };
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {

            if (txtConfirmPassword.Text.Trim() != txtNewPasword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _User = clsUser.Find(UserID);
            if ( _User != null )
            {
                ctrlUserCard1.LoadUserInfo(UserID);
            }
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

            _User.PassWord = clsGlobal.ComputeHash(txtNewPasword.Text.Trim());
            if (_User.Save())
            {
                MessageBox.Show("Password updated successfully","Success",MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _User.UserID);
                this.Close();
            }
            else
            {
                MessageBox.Show("Coudn't update password","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
