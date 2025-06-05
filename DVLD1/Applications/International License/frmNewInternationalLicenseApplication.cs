using DVLD1.Global_Classes;
using DVLD1.Licenses;
using DVLD1.Licenses.International_Licenses;
using DVLD1_Business;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Applications.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        public delegate void DataBackEventHandler(object sender);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;


        private int _SelectedLicenseID = -1;
        private int _LicenseID = -1;
        private clsLicense _LocalLicense;
        private int _InternationLicenseID = -1;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }
        public frmNewInternationalLicenseApplication(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
         private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
           
            _SelectedLicenseID = obj;
            if (_SelectedLicenseID == -1)
            {
                return;
            }
            _LocalLicense = clsLicense.Find(_SelectedLicenseID);

            _InternationLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(_LocalLicense.DriverID);
            if (_InternationLicenseID != -1)
            {
                lblLocalLicenseID.Text = _SelectedLicenseID.ToString();
                llShowLicenseHistory.Enabled = true;
                llShowLicenseInfo.Enabled = true;

                MessageBox.Show($"This person already have an active international license with ID = {_InternationLicenseID}", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_LocalLicense.LicenseClassIfo.LicenseClassID != 3)
            {
                lblLocalLicenseID.Text = _SelectedLicenseID.ToString();
                llShowLicenseHistory.Enabled = true;
                MessageBox.Show("Selected license shoud be class 3", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            lblLocalLicenseID.Text = _SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = true;
            btnIssueLicense.Enabled = true;
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_LocalLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            // Check if person  has a type 3 license
           
            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense.ApplicantPersonID = _LocalLicense.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationTypeID = (int) clsApplication.enApplicationType.NewInternationalLicense;
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
            InternationalLicense.CreatedByUserID =clsGlobal.CurrentUser.UserID;

            InternationalLicense.DriverID = _LocalLicense.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = _SelectedLicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            if (InternationalLicense.Save())
            {
                llShowLicenseInfo.Enabled = true;
                lblApplicationDate.Text = InternationalLicense.ApplicationID.ToString();
                lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
                DataBack?.Invoke(this);

                MessageBox.Show($"Interantiol license issued successfully with ID = {InternationalLicense.InternationalLicenseID}","License Issued",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo(_InternationLicenseID);
            frm.ShowDialog();
        }

        
        private void ctrlDriverLicenseInfoWithFilter1_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            if (_LicenseID != -1)
            {
                ctrlDriverLicenseInfoWithFilter1.Search(_LicenseID);
            }
            
        }
    }
}
