using DVLD1.Global_Classes;
using DVLD1.Licenses;
using DVLD1.Licenses.Local_Licenses;
using DVLD1_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.Rlease_Detained_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        public delegate void DataBackEventHandler(object sender);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        private int _LicenseID = -1;    
        private clsDetainedLicense _DetainedLicense;
        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            if (obj == -1)
            {
                llShowLicenseHistory.Enabled = false;
                return;
            }

            _DetainedLicense = clsDetainedLicense.FindByLicenseID(obj);



            if (!clsLicense.Find(_DetainedLicense.LicenseID).IsDetained)
            {
                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;
                MessageBox.Show("This licesene is not detained!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblDetainID.Text = _DetainedLicense.DetainID.ToString();
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();
            lblLicenseID.Text = _DetainedLicense.LicenseID.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblFineFees.Text = _DetainedLicense.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text.Trim()) + _DetainedLicense.FineFees).ToString();
            txtDetainReason.Text = _DetainedLicense.DetainReason;

            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            btnRelease.Enabled = true;
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {
            if (_LicenseID != -1)
                ctrlDriverLicenseInfoWithFilter1.Search(_LicenseID);
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

           clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = clsLicense.Find(_DetainedLicense.LicenseID).DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees;
            Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (!Application.Save() || !_DetainedLicense.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, Application.ApplicationID))
            {
                MessageBox.Show("Failed to release!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataBack?.Invoke(this);
            MessageBox.Show("Detained license released successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);


        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(clsLicense.Find(_DetainedLicense.LicenseID).DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_DetainedLicense.LicenseID);
            frm.ShowDialog();
        }
    }
}
