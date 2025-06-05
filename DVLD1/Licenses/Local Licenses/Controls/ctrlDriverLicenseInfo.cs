using DVLD1.Properties;
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

namespace DVLD1.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private clsLicense _License;
        private int _LicensesID;
        private void _LoadLicenseData()
        {
            lblClass.Text = _License.LicenseClassIfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _LicensesID.ToString();
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = _License.DriverInfo.PersonInfo.GendorText();
            lblIssueDate.Text = _License.IssueDate.ToShortDateString();
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes;
            lblIsActive.Text = _License.IsActive? "Yes" : "No";
            lblDateOfBirth.Text = _License.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToShortDateString();
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            pbPersonImage.ImageLocation = _License.DriverInfo.PersonInfo.ImagePath;
        }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public bool LoadData(int LicenseID)
        {
            _LicensesID = LicenseID;
            _License = clsLicense.Find(LicenseID);
            if( _License != null )
            {
                _LoadLicenseData();
                return true;
            }
            return false;
        }
        public void ResetControl()
        {
            lblClass.Text ="[???]";
            lblFullName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            lblIsDetained.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
        }
    }
}
