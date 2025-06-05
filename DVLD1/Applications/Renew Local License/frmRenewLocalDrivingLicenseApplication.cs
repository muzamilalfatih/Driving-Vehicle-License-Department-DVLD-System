using DVLD1.Global_Classes;
using DVLD1.Licenses;
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

namespace DVLD1.Applications.Renew_Local_License
{
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private clsLicense _OldLicense;
        private clsLicense _NewLicense;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {

            if (obj == -1)
            {
                llShowLicenseHistory.Enabled = false;
                return;
            }
            
            _OldLicense = clsLicense.Find(obj);
            
            if (!_OldLicense.IsActive)
            {
                llShowLicenseHistory.Enabled = true;
                MessageBox.Show("Selected license is Not Active, Choose and active license","Not Allowed",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
      
            lblOldLicenseID.Text = obj.ToString();
            lblLicenseFees.Text = _OldLicense.PaidFees.ToString();
            lblTotalFees.Text = (_OldLicense.PaidFees + (clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees)).ToString();

            if (!_OldLicense.IsLicenseExpired())
            {
                llShowLicenseHistory.Enabled = true;
                MessageBox.Show($"Selected License is not yet expired, it will expire on : {clsFormat.DateToShort(_OldLicense.ExpirationDate)}", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnRenewLicense.Enabled = true;
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.LicenseIDFocus();


            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;

            lblExpirationDate.Text = "???";
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_OldLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew the license ?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            _NewLicense = _OldLicense.RenewLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);
            if (_NewLicense != null)
            {
                ctrlDriverLicenseInfoWithFilter1.DiableFilter();
                lblApplicationID.Text = _NewLicense.ApplicationID.ToString();
                lblRenewedLicenseID.Text = _NewLicense.LicenseID.ToString();
                llShowLicenseInfo.Enabled = true;
                llShowLicenseHistory.Enabled = true;
                lblExpirationDate.Text = clsFormat.DateToShort(_NewLicense.ExpirationDate);
                btnRenewLicense.Enabled = false;
                MessageBox.Show($"License renewed successfully with ID = {_NewLicense.LicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_NewLicense.LicenseID);
            frm.ShowDialog();
        }
    }
}
