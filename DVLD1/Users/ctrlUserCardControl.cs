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

namespace DVLD1.Users
{
    public partial class ctrlUserCard : UserControl
    {
        clsUser _clsUser;
        public void LoadUserInfo(int UserID)
        {
            _clsUser = clsUser.Find(UserID);
            ctrlPersonCard1.LoadPersonInfo(_clsUser.PersonID);
            lblUserID.Text = _clsUser.UserID.ToString();
            lblUserName.Text = _clsUser.UserName;
            if (_clsUser.IsActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }
        public ctrlUserCard()
        {
            InitializeComponent();

        }
    }
}
