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

namespace DVLD1.Users
{
    public partial class frmUserInfo : Form
    {
        private int _UserID;
        
        public frmUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
    }
}
