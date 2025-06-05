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

namespace DVLD1.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _PersonID = -1;
       
        public frmShowPersonLicenseHistory(int PersonID)
        {
            _PersonID = PersonID;
            InitializeComponent();
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            clsDriver Driver = clsDriver.FindByPersonID(_PersonID);
            ctrlPersonCardWithFilter1.LoadPersonData(_PersonID);
            ctrlDriverLicenses1.LoadData(_PersonID);
        }
    }
}
