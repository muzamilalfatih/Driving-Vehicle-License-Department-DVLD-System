using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Licenses.International_Licenses
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        private int _InternationalLicenseID;
        public frmShowInternationalLicenseInfo(int internationalLicenseID)
        {
            InitializeComponent();
            _InternationalLicenseID = internationalLicenseID;
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            if (!ctrlDriverInternationalLicenseInfo1.LoadInfo(_InternationalLicenseID))
            {
                MessageBox.Show($"Coudn't find Internation License with ID = {_InternationalLicenseID}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
