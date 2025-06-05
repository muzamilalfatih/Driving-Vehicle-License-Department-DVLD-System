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

namespace DVLD1.Login
{
    public partial class frmLogin : Form
    {
        clsUser _User;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                
                MessageBox.Show("Some Fields are not filled!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            _User = clsUser.Find(txtUserName.Text.Trim(), clsGlobal.ComputeHash(txtPassword.Text.Trim()));
            if (_User != null )
            {
                if (_User.IsActive)
                {
                    if (chkRememberMe.Checked)
                    {
                        clsGlobal.RememberUsernameAndPassword(_User.UserName, txtPassword.Text.Trim());
                    }
                    else
                    {
                        clsGlobal.RememberUsernameAndPassword("", "");
                    }
                    clsGlobal.CurrentUser = _User;

                    this.Hide();
                    frmMain frm = new frmMain(this);
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Your account is not Active, Contact your admin","In Active Account",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid UserName/PassWord.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            };
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
                
                return;
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", PassWord = "";
            clsGlobal.GetStoredCredential(ref UserName, ref PassWord);
            txtUserName.Text = UserName;
            txtPassword.Text = PassWord;
        }

        
    }
}
