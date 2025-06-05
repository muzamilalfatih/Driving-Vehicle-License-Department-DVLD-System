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

namespace DVLD1.Pepole.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        public int PersonID = -1;

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }
        public void LoadPersonData(int PersonID)
        {
            comboBox1.SelectedIndex = 1;
            gbFilter.Enabled = false;
            txtFilterValue.Text = PersonID.ToString();
            this.PersonID = PersonID;
            ctrlPersonCard1.LoadPersonInfo(PersonID);

        }
        public void FilterFocus()
        {
           
            txtFilterValue.Focus();
        }
        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {
            
            comboBox1.SelectedIndex = 0;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                if (ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text))
                    PersonID = ctrlPersonCard1.PersonID;
            }
            else
            {
                if (ctrlPersonCard1.LoadPersonInfo(Convert.ToInt32((txtFilterValue.Text.Trim()))))
                    PersonID = ctrlPersonCard1.PersonID;
            }

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtFilterValue.Text == "Person ID")
            {
                if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNewUpdatePerson frm = new frmAddNewUpdatePerson();
            frm.DataBack += Form2_DataBack; // Subscribe to the event
            frm.ShowDialog();
        }
        private void Form2_DataBack(object sender, int PersonID)
        {
            if (PersonID != -1)
            {
                comboBox1.SelectedIndex = 1;
                txtFilterValue.Text = PersonID.ToString();
                ctrlPersonCard1.LoadPersonInfo(PersonID);
                this.PersonID = PersonID;
            }
            
        }
    }
}
