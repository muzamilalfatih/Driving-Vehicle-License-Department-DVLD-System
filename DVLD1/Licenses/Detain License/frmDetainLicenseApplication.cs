using DVLD1.classes;
using DVLD1.Global_Classes;
using DVLD1.Licenses.Local_Licenses;
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

namespace DVLD1.Licenses.Detain_License
{
    public partial class frmDetainLicenseApplication : Form
    {

        public delegate void DataBackEventHandler(object sender);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private clsLicense _License;
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            };


            if (!clsValidatoin.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);
            };
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            if (obj == -1)
            {
                llShowLicenseHistory.Enabled = false;
                return;
            }

            _License = clsLicense.Find(obj);

            if (!_License.IsActive)
            {
                llShowLicenseHistory.Enabled = true;
                MessageBox.Show("Selected license is Not Active, Choose and active license", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_License.IsDetained)
            {
                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;
                MessageBox.Show("This license is already detained!","Not Allowed",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLicenseID.Text = _License.LicenseID.ToString();
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            btnDetain.Enabled = true;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            txtFineFees.Validating += txtFineFees_Validating;
            txtDetainReason.Validating += txtDetainReason_Validating;
            if (!ValidateChildren())
                return;
            if (MessageBox.Show("Are you sure you want to detain this license?","Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            int DetainID = _License.Detain(Convert.ToSingle(txtFineFees.Text.Trim()),clsGlobal.CurrentUser.UserID,txtDetainReason.Text.Trim());
            if (DetainID == -1)
            {
                MessageBox.Show("Failed to detaie!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ctrlDriverLicenseInfoWithFilter1.DiableFilter();
            txtFineFees.Enabled = false;
            btnDetain.Enabled=false;
            txtDetainReason.Enabled = false;
            DataBack?.Invoke(this);
            MessageBox.Show($"License Detained successfully with DetainID {DetainID}","Detained",MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_License.LicenseID);
            frm.ShowDialog();
        }

        private void txtDetainReason_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDetainReason.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDetainReason, "Detain reason can't be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtDetainReason, null);

            }; 
        }
    }
}
