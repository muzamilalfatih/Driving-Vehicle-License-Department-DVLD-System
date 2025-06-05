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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD1.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl

    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID); // Raise the event with the parameter
            }
        }


        private int _LicenseID;
        private clsLicense _License;
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
           
        }

        
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {

                MessageBox.Show("Som fields are not valide; put the mouse over the red icon(s) to see the erro",
                                "Validation Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            if (int.TryParse(txtLicenseID.Text.Trim(),out _LicenseID))
            {
                if (ctrlDriverLicenseInfo1.LoadData(_LicenseID))
                {
                    OnLicenseSelected(_LicenseID);
                }
                else
                {
                    ctrlDriverLicenseInfo1.ResetControl();
                    MessageBox.Show($"Coudn't find the license with ID = {_LicenseID}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    OnLicenseSelected(-1);
                }
                    
            }else
            {
                return;
            }
            
            

        }
       
        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((txtLicenseID.Text.Trim())))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, "");
            }
        }
        
        public void Search(int LicenseID)
        {
            txtLicenseID.Text = LicenseID.ToString();
            gbFilters.Enabled = false;
            btnFind_Click(null,null);
        }
        public void LicenseIDFocus()
        {


            txtLicenseID.Focus().ToString();
        }
        public void DiableFilter()
        {
            gbFilters.Enabled=false;
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


            
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }
        }
    }
}
