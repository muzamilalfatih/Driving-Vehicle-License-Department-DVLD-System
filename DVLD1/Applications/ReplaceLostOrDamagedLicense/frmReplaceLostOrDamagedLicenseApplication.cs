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
using System.Xml;

namespace DVLD1.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private  clsLicense _OldLicense;
        private clsLicense _NewLicense;
        private clsLicense.enIssueReason _IssueReason = clsLicense.enIssueReason.DamagedReplacement;
        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }
        private void _UpdateTitil()
        {
            

            if (_IssueReason == clsLicense.enIssueReason.DamagedReplacement)
            {
                lblTitle.Text = "Replacement for Damaged License";
            }
            else lblTitle.Text = "ReplaceMent For Lost License"; 

        }
        private void _UpdateIssueReason()
        {
            _IssueReason  = rbDamagedLicense.Checked ?  clsLicense.enIssueReason.DamagedReplacement :  clsLicense.enIssueReason.LostReplacement;
        }
        private void _UpdateApplicationFees()
        {
            lblApplicationFees.Text =
                rbDamagedLicense.Checked ?
                clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).Fees.ToString()
                : clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).Fees.ToString();
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
                lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
                MessageBox.Show("Selected license is Not Active, Choose and active license", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            llShowLicenseHistory.Enabled = true;
            btnIssueReplacement.Enabled = true;
                  
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.LicenseIDFocus();
            _UpdateTitil();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            _UpdateApplicationFees();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateIssueReason();
            _UpdateTitil();
            _UpdateApplicationFees();
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
           if ( MessageBox.Show("Are you sure you want to issue a replacement for the license?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No )
            {
                return;
            }
            _NewLicense = _OldLicense.Replace(_IssueReason, clsGlobal.CurrentUser.UserID);
            if (_NewLicense != null)
            {
                llShowLicenseHistory.Enabled=true;
                llShowLicenseInfo.Enabled=true;
                lblApplicationID.Text = _NewLicense.ApplicationID.ToString();
                lblRreplacedLicenseID.Text = _NewLicense.LicenseID.ToString();
                btnIssueReplacement.Enabled = false;
                ctrlDriverLicenseInfoWithFilter1.DiableFilter();
                gbReplacementFor.Enabled=false;
                MessageBox.Show($"License replaced successfully with is {_NewLicense.LicenseID}","License Issued",MessageBoxButtons.OK,MessageBoxIcon.Information );
            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowLicenseInfo(_NewLicense.LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_OldLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
