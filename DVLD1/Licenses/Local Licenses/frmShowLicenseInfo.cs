﻿using DVLD1_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Licenses.Local_Licenses
{
    public partial class frmShowLicenseInfo : Form
    {
        private int _LicenseID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmShowLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadData(_LicenseID);
        }
    }
}
