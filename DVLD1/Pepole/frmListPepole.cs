using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD1_Business;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD1.Pepole
{
    public partial class frmListPepole : Form
    {
        private static DataTable _dtAllPeople = null;
        private DataTable _dtPeople = null;
        public frmListPepole()
        {
            InitializeComponent();
        }
        private void _RefreshPerpleDataTable()
        {
            _dtAllPeople = clsPerson.GetAllPeople(); ;
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                     "FirstName", "SecondName", "ThirdName", "LastName",
                                                     "GendorCaption", "DateOfBirth", "CountryName",
                                                     "Phone", "Email");
            dgvPeople.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }
        }
        private void frmListPepole_Load(object sender, EventArgs e)
        {

            _RefreshPerpleDataTable();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
            if (txtFilterValue.Visible )
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();

                

            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }
            

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
            {
                if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddNewUpdatePerson frm = new frmAddNewUpdatePerson();
            frm.DataBack += Form2_DataBack; // Subscribe to the event
            frm.ShowDialog();
        }

        private void Form2_DataBack(object sender, int PersonID)
        {
            _RefreshPerpleDataTable();
        }

        private void showDtailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = 0;
            PersonID = (int) dgvPeople.CurrentRow.Cells[0].Value;
            frmPersonInfo frm = new frmPersonInfo(PersonID);
            frm.ShowDialog();

            
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewUpdatePerson frm = new frmAddNewUpdatePerson();
            frm.DataBack += Form2_DataBack; // Subscribe to the event
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmAddNewUpdatePerson frm = new frmAddNewUpdatePerson(PersonID);
            frm.DataBack += Form2_DataBack; // Subscribe to the event
            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            if (MessageBox.Show($"Are you sure you want to delete person[{PersonID}]",
                "Confirm",
                MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsPerson.DeletePerson(PersonID))
                {
                    _RefreshPerpleDataTable();
                    MessageBox.Show("Person Deleted Successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet!","Not Ready",MessageBoxButtons.OK,MessageBoxIcon.Warning);
        }

        private void phoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
